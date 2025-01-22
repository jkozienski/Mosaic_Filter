.data
    align 16
    mask_b dd 000000FFh, 000000FFh, 000000FFh, 000000FFh  ; Maska dla B
    mask_g dd 0000FF00h, 0000FF00h, 0000FF00h, 0000FF00h  ; Maska dla G
    mask_r dd 00FF0000h, 00FF0000h, 00FF0000h, 00FF0000h  ; Maska dla R

.code
ApplyMosaicASM proc
    ; Parametry:
    ; RCX - wskaźnik do bufora wejściowego (InBuffer)
    ; RDX - wskaźnik do bufora wyjściowego (OutBuffer) 
    ; R8D - wysokość obrazu (height)
    ; R9D - szerokość obrazu (width)
    ; [RSP+48] - początkowy offset dla wątku (start)     
    ; [RSP+56] - końcowy offset dla wątku (end)
    ; [RSP+64] - rozmiar kafelka (tileSize)

    push rbp
    mov rbp, rsp
    push rbx
    push rsi
    push rdi
    push r12
    push r13
    push r14
    push r15

    ; parametry do rejestrów
    mov rsi, rcx                     ; RSI - wskaźnik na bufor źródłowy
    mov rdi, rdx                     ; RDI - wskaźnik na bufor wynikowy 
    mov r12d, r8d                    ; R12D - wysokość obrazu
    mov r13d, r9d                    ; R13D - szerokość obrazu
    mov r14d, dword ptr [rbp+48]     ; R14D - startIndex (offset początku dla wątku)
    mov r15d, dword ptr [rbp+56]     ; R15D - finishIndex (offset końca dla wątku)
    mov ebx, [rbp+64]                ; EBX - rozmiar kafelka (tileSize)


     ; Pętla po wierszach kafelków
tile_row_loop:
    cmp r14d, r12d            ; Sprawdza przekroczenie wysokości
    jge done

    xor r15d, r15d            ;   kolumna kafelka (x)
tile_col_loop:
    cmp r15d, r13d            ; Sprawdz przekroczenie szerokości
    jge next_tile_row

   
     ; Inicjalizacja sum kolorów dla kafelka
    pxor xmm5, xmm5           ;  sumy składowej B
    pxor xmm6, xmm6           ;  sumy składowej G
    pxor xmm7, xmm7           ;  sumy składowej R
    xor r8d, r8d              ; Licznik pikseli w kafelku

  ; Iteracja po pikselach w kafelku
    xor r10d, r10d                   ; y-offset w kafelku
pixel_row_loop:
    mov eax, r14d
    add eax, r10d                    ; Aktualny y = wiersz_kafelka + y_offset
    cmp eax, r12d                    ; Sprawdza czy nie wyszło poza obraz
    jge calc_average
    cmp r10d, ebx                    ; Sprawdza czy nie wyszło poza kafelek
    jge calc_average

    xor r11d, r11d                   ; x-offset w kafelku
pixel_col_loop:
    mov eax, r15d
    add eax, r11d                    ; Aktualny x = kolumna_kafelka + x_offset
    cmp eax, r13d                    ; Sprawdza czy nie wyszło poza obraz
    jge pixel_row_next
    cmp r11d, ebx                    ; Sprawdza czy nie wyszło poza kafelek
    jge pixel_row_next

    ; Oblicz pozycję piksela
    mov eax, r14d
    add eax, r10d            ; Wiersz + offset
    imul eax, r13d           ; * szerokość
    add eax, r15d            ; + kolumna
    add eax, r11d            ; + offset kolumny
    shl eax, 2              ; * 4 (BGRA)

    ;SIMD
    ; Wczytaj 4 piksele na raz (16 bajtów - BGRA)
    movdqu xmm0, [rsi + rax]    ; Wczytaj 4 piksele BGRA naraz

    ; Wyodrębnia i przetwórza składową B
    movdqa xmm1, xmm0           ; Kopiowanie 4 pikseli do xmm1
    pand xmm1, xmmword ptr [mask_b]  ; zostawia tylko  B
    cvtdq2ps xmm1, xmm1         ; Konwertuje 4 wartości int na float
    addps xmm5, xmm1            ; 

    ;  przetwórz  G
    movdqa xmm2, xmm0           
    pand xmm2, xmmword ptr [mask_g]  
    psrld xmm2, 8               ; Przesunięcie bitowe  
    cvtdq2ps xmm2, xmm2        
    addps xmm6, xmm2            

    ;  przetwórz  R
    movdqa xmm3, xmm0          
    pand xmm3, xmmword ptr [mask_r]  
    psrld xmm3, 16                ; Przesunięcie bitowe  
    cvtdq2ps xmm3, xmm3        
    addps xmm7, xmm3            

    add r8d, 4                  ; Zwiększ licznik o 4 piksele
    add r11d, 4                 ; Przejdź do następnych 4 pikseli
    jmp pixel_col_loop

pixel_row_next:
    inc r10d                   ; kolejny wiersz
    jmp pixel_row_loop

calc_average:
    test r8d, r8d              ; Sprawdza czy mamy jakieś piksele
    jz next_tile_col

    ;SIMD
    ; sumuje wszystkie składowe w rejestrach wektorowych
    movaps xmm1, xmm5          ; Suma B
    haddps xmm1, xmm1          ; Suma  (1+2, 3+4)
    haddps xmm1, xmm1          ; Suma  (12+34)
    
    movaps xmm2, xmm6          ; Suma G
    haddps xmm2, xmm2
    haddps xmm2, xmm2

    movaps xmm3, xmm7          ; Suma R
    haddps xmm3, xmm3
    haddps xmm3, xmm3

    ; konwersja licznika na float i dzielenie
    cvtsi2ss xmm0, r8d
    shufps xmm0, xmm0, 0       ; Skopiuj wartość do wszystkich elementów

    ; Oblicaz średnie
    divss xmm1, xmm0           ; Średnia B
    divss xmm2, xmm0           ; Średnia G
    divss xmm3, xmm0           ; Średnia R

    ; Konwertuje wyniki na liczby całkowite
    cvttss2si ecx, xmm1
    mov r8b, cl                ; Zachowuje średnią B
    cvttss2si ecx, xmm2
    mov r9b, cl                ; Zachowuje średnią G
    cvttss2si ecx, xmm3
    mov r10b, cl               ; Zachowuje średnią R

    ; Zapisuje średnie wartości do kafelka
    xor r11d, r11d            ; Resetuje licznika wierszy
write_row_loop:
    mov eax, r14d
    add eax, r11d             ; Aktualny wiersz
    cmp eax, r12d            ; Sprawdza czy nie wyszło poza obraz
    jge next_tile_col
    cmp r11d, ebx            ; Sprawdza czy nie wyszło poza kafelek
    jge next_tile_col           
                                
    xor ecx, ecx              ; Reset licznika kolumn
write_col_loop:
    mov eax, r15d
    add eax, ecx              ; Aktualna kolumna
    cmp eax, r13d            ; Sprawdza czy nie wyszło poza obraz
    jge write_row_next
    cmp ecx, ebx             ; Sprawdza czy nie wyszło poza kafelek
    jge write_row_next

    ; Oblicaz pozycję do zapisu w obrazie
    mov eax, r14d
    add eax, r11d            ; Wiersz + offset
    imul eax, r13d           ; * szerokość
    add eax, r15d            ; + kolumna
    add eax, ecx             ; + offset kolumny
    shl eax, 2              ; * 4 (BGRA)

    ; Zapisz piksele
    mov byte ptr [rdi + rax], r8b        ; B
    mov byte ptr [rdi + rax + 1], r9b    ; G
    mov byte ptr [rdi + rax + 2], r10b   ; R
    mov byte ptr [rdi + rax + 3], 255    ; A

    inc ecx                    ; Następna kolumna
    jmp write_col_loop

write_row_next:
    inc r11d                   ; Następny wiersz
    jmp write_row_loop

next_tile_col:
    add r15d, ebx             ; Następny kafelek w wierszu
    jmp tile_col_loop

next_tile_row:
    add r14d, ebx             ; Następny wiersz kafelków
    jmp tile_row_loop

done:
    pop r15
    pop r14
    pop r13
    pop r12
    pop rdi
    pop rsi
    pop rbx
    pop rbp
    ret

ApplyMosaicASM endp
end
