.data
    align 16
    zero_vector dd 0, 0, 0, 0
    mask_b dd 000000FFh, 0, 0, 0    ; Maska dla kana³u niebieskiego
    mask_g dd 0000FF00h, 0, 0, 0    ; Maska dla kana³u zielonego
    mask_r dd 00FF0000h, 0, 0, 0    ; Maska dla kana³u czerwonego

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
    mov rsi, rcx               ; RSI = wskaŸnik na bufor wejœciowy
    mov rdi, rdx               ; RDI = wskaŸnik na bufor wyjœciowy
    mov r12d, r8d             ; R12D = wysokoœæ obrazu
    mov r13d, r9d             ; R13D = szerokoœæ obrazu
    mov ebx, [rbp+64]         ; EBX = rozmiar kafelka

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

    ; Wczytaj kolory
    movzx ecx, byte ptr [rsi + rax]      ; B
    cvtsi2ss xmm0, ecx
    addss xmm5, xmm0                     ; Dodaj do sumy B

    movzx ecx, byte ptr [rsi + rax + 1]  ; G
    cvtsi2ss xmm0, ecx
    addss xmm6, xmm0                     ; Dodaj do sumy G

    movzx ecx, byte ptr [rsi + rax + 2]  ; R
    cvtsi2ss xmm0, ecx
    addss xmm7, xmm0                     ; Dodaj do sumy R

    inc r8d                    ; Zwiêksz licznik pikseli
    inc r11d                   ; Nastêpna kolumna
    jmp pixel_col_loop

pixel_row_next:
    inc r10d                   ; Nastêpny wiersz
    jmp pixel_row_loop

calc_average:
    ; Oblicz œrednie
    test r8d, r8d              ; SprawdŸ czy mamy jakieœ piksele
    jz next_tile_col
    
    cvtsi2ss xmm4, r8d        ; Konwertuj licznik na float
    
    ; Oblicz œrednie wartoœci
    divss xmm5, xmm4          ; Œrednia B
    divss xmm6, xmm4          ; Œrednia G
    divss xmm7, xmm4          ; Œrednia R

    ; Konwertuj na ca³kowite
    cvttss2si ecx, xmm5
    mov r8b, cl               ; Zachowaj B
    cvttss2si ecx, xmm6
    mov r9b, cl               ; Zachowaj G
    cvttss2si ecx, xmm7
    mov r10b, cl              ; Zachowaj R

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