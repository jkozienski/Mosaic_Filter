.data
    align 16
    mask_b dd 000000FFh, 000000FFh, 000000FFh, 000000FFh  ; Maska dla B
    mask_g dd 0000FF00h, 0000FF00h, 0000FF00h, 0000FF00h  ; Maska dla G
    mask_r dd 00FF0000h, 00FF0000h, 00FF0000h, 00FF0000h  ; Maska dla R

.code
ApplyMosaicASM proc
    ; RCX - InBuffer
    ; RDX - OutBuffer
    ; R8D - height
    ; R9D - width
    ; [RSP+48] - start     
    ; [RSP+56] - end
    ; [RSP+64] - tileSize

    push rbp
    mov rbp, rsp
    push rbx
    push rsi
    push rdi
    push r12
    push r13
    push r14
    push r15

    ; Zachowanie parametrów 
    mov rsi, rcx                     ; RSI = wskaŸnik na bufor wejœciowy
    mov rdi, rdx                   ; RDI = wskaŸnik na bufor wyjœciowy
    mov r12d, r8d             ; R12D = wysokoœæ obrazu
    mov r13d, r9d             ; R13D = szerokoœæ obrazu
    mov r14d, dword ptr [rbp+48]  ; R14D = Start offset
    mov r15d, dword ptr [rbp+56]  ; R15D = Start offset
    mov ebx, [rbp+64]         ; EBX = rozmiar kafelka

    ; Oblicz wiersz pocz¹tkowy i koñcowy
    mov eax, r14d
    shr eax, 2                ; Dziel przez 4 (BGRA)
    xor edx, edx
    div r13d                  ; Dziel przez szerokoœæ
    mov r14d, eax            ; R14D = wiersz pocz¹tkowy

    mov eax, r15d
    shr eax, 2                ; Dziel przez 4 (BGRA)
    xor edx, edx
    div r13d                  ; Dziel przez szerokoœæ
    mov r15d, eax            ; R15D = wiersz koñcowy

    ; Inicjalizacja liczników kafelków
    xor r14d, r14d            ; r14d = aktualny wiersz kafelka
tile_row_loop:
    cmp r14d, r12d            ; SprawdŸ czy nie przekroczyliœmy wysokoœci
    jge done

    xor r15d, r15d            ; r15d = aktualna kolumna kafelka
tile_col_loop:
    cmp r15d, r13d            ; SprawdŸ czy nie przekroczyliœmy szerokoœci
    jge next_tile_row

    ; Inicjalizacja sum kolorów
    pxor xmm5, xmm5           ; Suma dla B
    pxor xmm6, xmm6           ; Suma dla G
    pxor xmm7, xmm7           ; Suma dla R
    xor r8d, r8d              ; Licznik pikseli

    ; Iteracja po pikselach w kafelku
    xor r10d, r10d            ; Offset wiersza w kafelku
pixel_row_loop:
    mov eax, r14d
    add eax, r10d             ; Aktualny wiersz
    cmp eax, r12d            ; SprawdŸ czy nie wyszliœmy poza obraz
    jge calc_average
    cmp r10d, ebx            ; SprawdŸ czy nie wyszliœmy poza kafelek
    jge calc_average

    xor r11d, r11d            ; Offset kolumny w kafelku
pixel_col_loop:
    mov eax, r15d
    add eax, r11d             ; Aktualna kolumna
    cmp eax, r13d            ; SprawdŸ czy nie wyszliœmy poza obraz
    jge pixel_row_next
    cmp r11d, ebx            ; SprawdŸ czy nie wyszliœmy poza kafelek
    jge pixel_row_next

    ; Oblicz pozycjê piksela
    mov eax, r14d
    add eax, r10d            ; Wiersz + offset
    imul eax, r13d           ; * szerokoœæ
    add eax, r15d            ; + kolumna
    add eax, r11d            ; + offset kolumny
    shl eax, 2              ; * 4 (BGRA)

    ; Wczytaj 4 piksele na raz (16 bajtów - BGRABGRABGRABGRA)
    movdqu xmm0, [rsi + rax]    ; Wczytaj 4 piksele BGRA naraz

    ; Wyodrêbnij i przetwórz sk³adow¹ B
    movdqa xmm1, xmm0           ; Kopia danych
    pand xmm1, xmmword ptr [mask_b]  ; Wyodrêbnij sk³adow¹ B
    cvtdq2ps xmm1, xmm1         ; Konwertuj 4 wartoœci B na float
    addps xmm5, xmm1            ; Dodaj 4 wartoœci B do sumy

    ; Wyodrêbnij i przetwórz sk³adow¹ G
    movdqa xmm2, xmm0           ; Kopia danych
    pand xmm2, xmmword ptr [mask_g]  ; Wyodrêbnij sk³adow¹ G
    psrld xmm2, 8               ; Przesuñ G do pozycji najm³odszego bajtu
    cvtdq2ps xmm2, xmm2         ; Konwertuj 4 wartoœci G na float
    addps xmm6, xmm2            ; Dodaj 4 wartoœci G do sumy

    ; Wyodrêbnij i przetwórz sk³adow¹ R
    movdqa xmm3, xmm0           ; Kopia danych
    pand xmm3, xmmword ptr [mask_r]  ; Wyodrêbnij sk³adow¹ R
    psrld xmm3, 16              ; Przesuñ R do pozycji najm³odszego bajtu
    cvtdq2ps xmm3, xmm3         ; Konwertuj 4 wartoœci R na float
    addps xmm7, xmm3            ; Dodaj 4 wartoœci R do sumy

    add r8d, 4                  ; Zwiêksz licznik o 4 piksele
    add r11d, 4                 ; PrzejdŸ do nastêpnych 4 pikseli
    jmp pixel_col_loop

pixel_row_next:
    inc r10d                   ; Nastêpny wiersz
    jmp pixel_row_loop

calc_average:
    test r8d, r8d              ; SprawdŸ czy mamy jakieœ piksele
    jz next_tile_col

    ; Zsumuj wszystkie sk³adowe w rejestrach wektorowych
    movaps xmm1, xmm5          ; Suma B
    haddps xmm1, xmm1          ; Suma horyzontalna (1+2, 3+4)
    haddps xmm1, xmm1          ; Suma horyzontalna (12+34)
    
    movaps xmm2, xmm6          ; Suma G
    haddps xmm2, xmm2
    haddps xmm2, xmm2

    movaps xmm3, xmm7          ; Suma R
    haddps xmm3, xmm3
    haddps xmm3, xmm3

    ; Konwertuj licznik na float i przygotuj do dzielenia
    cvtsi2ss xmm0, r8d
    shufps xmm0, xmm0, 0       ; Skopiuj wartoœæ do wszystkich elementów

    ; Oblicz œrednie
    divss xmm1, xmm0           ; Œrednia B
    divss xmm2, xmm0           ; Œrednia G
    divss xmm3, xmm0           ; Œrednia R

    ; Konwertuj wyniki na liczby ca³kowite
    cvttss2si ecx, xmm1
    mov r8b, cl                ; Zachowaj œredni¹ B
    cvttss2si ecx, xmm2
    mov r9b, cl                ; Zachowaj œredni¹ G
    cvttss2si ecx, xmm3
    mov r10b, cl               ; Zachowaj œredni¹ R

    ; Zapisz œrednie wartoœci do kafelka
    xor r11d, r11d            ; Reset licznika wierszy
write_row_loop:
    mov eax, r14d
    add eax, r11d             ; Aktualny wiersz
    cmp eax, r12d            ; SprawdŸ czy nie wyszliœmy poza obraz
    jge next_tile_col
    cmp r11d, ebx            ; SprawdŸ czy nie wyszliœmy poza kafelek
    jge next_tile_col

    xor ecx, ecx              ; Reset licznika kolumn
write_col_loop:
    mov eax, r15d
    add eax, ecx              ; Aktualna kolumna
    cmp eax, r13d            ; SprawdŸ czy nie wyszliœmy poza obraz
    jge write_row_next
    cmp ecx, ebx             ; SprawdŸ czy nie wyszliœmy poza kafelek
    jge write_row_next

    ; Oblicz pozycjê do zapisu
    mov eax, r14d
    add eax, r11d            ; Wiersz + offset
    imul eax, r13d           ; * szerokoœæ
    add eax, r15d            ; + kolumna
    add eax, ecx             ; + offset kolumny
    shl eax, 2              ; * 4 (BGRA)

    ; Zapisz piksele
    mov byte ptr [rdi + rax], r8b        ; B
    mov byte ptr [rdi + rax + 1], r9b    ; G
    mov byte ptr [rdi + rax + 2], r10b   ; R
    mov byte ptr [rdi + rax + 3], 255    ; A

    inc ecx                    ; Nastêpna kolumna
    jmp write_col_loop

write_row_next:
    inc r11d                   ; Nastêpny wiersz
    jmp write_row_loop

next_tile_col:
    add r15d, ebx             ; Nastêpny kafelek w wierszu
    jmp tile_col_loop

next_tile_row:
    add r14d, ebx             ; Nastêpny wiersz kafelków
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