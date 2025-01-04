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

    ; Zapisanie parametrów
    mov rsi, rcx               ; RSI  wskaŸnik na bufor wejœciowy
    mov rdi, rdx               ; RDI  wskaŸnik na bufor wyjœciowy
    mov r12d, r8d              ; R12D  wysokoœæ obrazu
    mov r13d, r9d              ; R13D  szerokoœæ obrazu
    
    mov r14d, dword ptr [rbp+48]    ; R14D  indeks startowy
    mov r15d, dword ptr [rbp+56]    ; R15D  indeks koñcowy
    mov ebx, dword ptr [rbp+64]     ; EBX  rozmiar kafelka
    
    ; Zewnêtrzna pêtla po kafelkach (wiersze)
tile_row_loop:
    cmp r14d, r15d        ; SprawdŸ czy nie przekroczyliœmy koñcowego indeksu
    jge done              ; Jeœli tak, zakoñcz

    mov eax, r14d         ; Oblicz wiersz kafelka
    xor edx, edx         
    div r13d             ; EAX = wiersz (y / width)
    mov r8d, eax         ; R8D = aktualny wiersz kafelka
    imul r8d, ebx        ; R8D *= tileSize (pocz¹tek wiersza kafelka)

    ; Wewnêtrzna pêtla po kafelkach (kolumny)
    mov eax, r14d
    xor edx, edx
    div r13d             ; EDX = kolumna (y % width)
    mov r9d, edx         ; R9D = aktualna kolumna kafelka
    imul r9d, ebx        ; R9D *= tileSize (pocz¹tek kolumny kafelka)

    ; Iteracja po pikselach w kafelku (wiersze)
    xor r10d, r10d       ; R10D = offset wiersza w kafelku
pixel_row_loop:
    cmp r10d, ebx        ; Porównaj z rozmiarem kafelka
    jge next_tile        ; Jeœli >= tileSize, przejdŸ do nastêpnego kafelka

    ; SprawdŸ czy nie wyszliœmy poza obraz
    mov eax, r8d
    add eax, r10d        ; Aktualny wiersz obrazu
    cmp eax, r12d        ; Porównaj z wysokoœci¹ obrazu
    jge next_tile

        ; Wczytaj wartoœci kolorów do rejestrów wektorowych
    pxor xmm0, xmm0     ; Wyzeruj rejestr dla R
    pxor xmm1, xmm1     ; Wyzeruj rejestr dla G
    pxor xmm2, xmm2     ; Wyzeruj rejestr dla B
    pxor xmm3, xmm3     ; Wyzeruj rejestr pomocniczy

    pxor xmm5, xmm5    ; Suma dla B
    pxor xmm6, xmm6    ; Suma dla G
    pxor xmm7, xmm7    ; Suma dla R
    xor r8d, r8d        ; Licznik pikseli w kafelku (nowy rejestr)
    ; Iteracja po pikselach w kafelku (kolumny)
    xor r11d, r11d       ; R11D = offset kolumny w kafelku
pixel_col_loop:
    cmp r11d, ebx        ; Porównaj z rozmiarem kafelka
    jge pixel_row_next   ; Jeœli >= tileSize, przejdŸ do nastêpnego wiersza

    ; SprawdŸ czy nie wyszliœmy poza obraz
    mov eax, r9d
    add eax, r11d        ; Aktualna kolumna obrazu
    cmp eax, r13d        ; Porównaj z szerokoœci¹ obrazu
    jge pixel_row_next

; Oblicz pozycjê piksela: [wiersz * szerokoœæ + kolumna] * 4 (dla 32bpp)
    mov eax, r8d         ; Wiersz kafelka
    add eax, r10d        ; + offset wiersza w kafelku
    imul eax, r13d       ; * szerokoœæ obrazu
    add eax, r9d         ; + kolumna kafelka
    add eax, r11d        ; + offset kolumny w kafelku
    shl eax, 2          ; * 4 (konwersja na bajty dla 32bpp)

    ; Wczytaj kolory z bufora wejœciowego
    movzx ecx, byte ptr [rsi + rax]      ; Wczytaj B
    cvtsi2ss xmm2, ecx                   ; Konwertuj B na float
    addss xmm2, xmm5                     ; Dodaj do sumy B (xmm5 to suma B)

    movzx ecx, byte ptr [rsi + rax + 1]  ; Wczytaj G
    cvtsi2ss xmm1, ecx                   ; Konwertuj G na float
    addss xmm1, xmm6                     ; Dodaj do sumy G (xmm6 to suma G)

    movzx ecx, byte ptr [rsi + rax + 2]  ; Wczytaj R
    cvtsi2ss xmm0, ecx                   ; Konwertuj R na float
    addss xmm0, xmm7                     ; Dodaj do sumy R (xmm7 to suma R)

    inc r8d            ; Zwiêksz licznik pikseli
    inc r11d           ; Nastêpna kolumna w kafelku
    jmp pixel_col_loop

pixel_row_next:
    inc r10d            ; Nastêpny wiersz w kafelku
    jmp pixel_row_loop

next_tile:
; Oblicz œredni¹ dla kafelka
    cvtsi2ss xmm3, r8d     ; Konwertuj licznik na float
    movss xmm0, xmm7       ; Przenieœ sumê R
    movss xmm1, xmm6       ; Przenieœ sumê G
    movss xmm2, xmm5       ; Przenieœ sumê B
    
    divss xmm0, xmm3       ; Œrednia Red
    divss xmm1, xmm3       ; Œrednia Green
    divss xmm2, xmm3       ; Œrednia Blue

    ; Konwersja z powrotem na ca³kowite
    cvttss2si ecx, xmm2    ; Blue
    mov r15b, cl           ; Zachowaj B
    cvttss2si ecx, xmm1    ; Green
    mov r14b, cl           ; Zachowaj G
    cvttss2si ecx, xmm0    ; Red
    mov r13b, cl           ; Zachowaj R

    ; Zresetuj licznik pikseli dla nastêpnego kafelka
    finish_write:
    pxor xmm5, xmm5    ; Wyzeruj sumê B
    pxor xmm6, xmm6    ; Wyzeruj sumê G
    pxor xmm7, xmm7    ; Wyzeruj sumê R
    xor r8d, r8d       ; Wyzeruj licznik pikseli

    inc r14d            ; PrzejdŸ do nastêpnego kafelka
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