.data
    align 16
    zero_vector dd 0, 0, 0, 0
    mask_b dd 000000FFh, 0, 0, 0    ; Maska dla kana≥u niebieskiego
    mask_g dd 0000FF00h, 0, 0, 0    ; Maska dla kana≥u zielonego
    mask_r dd 00FF0000h, 0, 0, 0    ; Maska dla kana≥u czerwonego
.code
ApplyMosaicASM proc
    ; Parametry:
    ; RCX - wskaünik do danych obrazu (sourcePtr)
    ; RDX - stride (szerokoúÊ linii w bajtach)
    ; R8  - szerokoúÊ obrazu (width)
    ; R9  - wysokoúÊ obrazu (height)
    ; [RSP+40] - rozmiar kafelka (tileSize)

    ; Zachowanie rejestrÛw
    push rbp
    mov rbp, rsp
    push rsi
    push rdi
    push rbx
    push r12
    push r13
    push r14
    push r15
    ; Inicjalizacja rejestrÛw XMM
    pxor xmm4, xmm4        ; Wyzeruj xmm4 do uøycia jako maska

    ; Zachowanie argumentÛw w bezpiecznych rejestrach
    mov rsi, rcx            ; sourcePtr
    mov rdi, rdx            ; stride
    mov r10, r8             ; width
    mov r11, r9             ; height
    mov r12, [rbp+48]       ; tileSize



    ; Sprawdü czy stride jest poprawny
    cmp rdi, r10                     ; stride >= width?
    jl done

    ; Iteracja po kafelkach
    xor r13, r13            ; tile_y = 0
tile_y_loop:
    cmp r13, r11            ; Sprawdü czy tile_y < height
    jge done

    xor r14, r14            ; tile_x = 0
tile_x_loop:
    cmp r14, r10            ; Sprawdü czy tile_x < width
    jge next_tile_row

    ; Oblicz rozmiar aktualnego kafelka
    mov r15, r12            ; current_tile_width = tileSize
    mov rbx, r10
    sub rbx, r14            ; remaining_width = width - tile_x
    cmp r15, rbx
    jle use_current_width
    mov r15, rbx            ; Uøyj remaining_width jeúli jest mniejsze
use_current_width:

    ; Inicjalizacja sum kolorÛw dla kafelka
    pxor xmm1, xmm1        ; Suma B (dolne 32 bity)
    pxor xmm2, xmm2        ; Suma G (dolne 32 bity)
    pxor xmm3, xmm3        ; Suma R (dolne 32 bity)
    xor r8d, r8d           ; Licznik pikseli

    ; Iteracja po pikselach w kafelku
    xor r8, r8              ; y = 0
pixel_y_loop:
    cmp r8, r15             ; Sprawdü czy y < current_tile_width
    jge calc_average

    ; Oblicz adres poczπtku wiersza
    mov rax, r13
    add rax, r8             ; y + tile_y
    mul rdi                 ; * stride
    lea rax, [rsi + rax]    ; adres poczπtku wiersza

    xor r9, r9              ; x = 0

pixel_x_loop:
    cmp r9, r15
    jge next_pixel_y

    ; Oblicz adres piksela
    mov rcx, r14
    add rcx, r9
    lea rcx, [rax + rcx*4]

    ; Wczytaj piksel
    movd xmm0, dword ptr [rcx]      ; Wczytaj BGRA

    ; WyodrÍbnij poszczegÛlne kana≥y
    movdqa xmm5, xmm0               ; Kopia dla B
    movdqa xmm6, xmm0               ; Kopia dla G
    movdqa xmm7, xmm0               ; Kopia dla R

    ; WyodrÍbnij B (juø jest na w≥aúciwej pozycji)
    pand xmm5, oword ptr [mask_b]  ; Zostaw tylko B
    paddd xmm1, xmm5                ; Dodaj do sumy B

    ; WyodrÍbnij G
    pand xmm6, oword ptr [mask_g]  ; Zostaw tylko G
    psrld xmm6, 8                   ; PrzesuÒ na w≥aúciwπ pozycjÍ
    paddd xmm2, xmm6                ; Dodaj do sumy G

    ; WyodrÍbnij R
    pand xmm7, oword ptr [mask_r]  ; Zostaw tylko R
    psrld xmm7, 16                  ; PrzesuÒ na w≥aúciwπ pozycjÍ
    paddd xmm3, xmm7                ; Dodaj do sumy R

    inc r8d
    inc r9
    jmp pixel_x_loop

next_pixel_y:
    inc r8                  ; y++
    jmp pixel_y_loop

calc_average:
    ; Oblicz úrednie
    mov eax, r8d                    ; Liczba pikseli w kafelku
    cvtsi2ss xmm4, eax             ; Konwertuj na float
    shufps xmm4, xmm4, 0           ; Skopiuj wartoúÊ do wszystkich elementÛw

    ; Konwertuj sumy na float i podziel
    cvtdq2ps xmm1, xmm1            ; B
    cvtdq2ps xmm2, xmm2            ; G
    cvtdq2ps xmm3, xmm3            ; R

    divps xmm1, xmm4               ; årednia B
    divps xmm2, xmm4               ; årednia G
    divps xmm3, xmm4               ; årednia R

    ; Konwertuj z powrotem na inty
    cvtps2dq xmm1, xmm1
    cvtps2dq xmm2, xmm2
    cvtps2dq xmm3, xmm3

   ; Wype≥nij kafelek úrednimi wartoúciami
    xor r8, r8                       ; y = 0
fill_y_loop:
    cmp r8, r15                      ; Sprawdü czy y < current_tile_width
    jge next_tile_x

    ; Oblicz adres poczπtku wiersza bezpiecznie
    mov rax, r13                     ; tile_y
    add rax, r8                      ; + current y
    cmp rax, r11                     ; Sprawdü czy nie przekraczamy height
    jge next_tile_x
    
    mul rdi                          ; * stride
    mov rbx, rax                     ; Zachowaj offset wiersza
    
    xor r9, r9                       ; x = 0
fill_x_loop:
    cmp r9, r15                      ; Sprawdü czy x < current_tile_width
    jge fill_next_y

    ; Oblicz offset piksela bezpiecznie
    mov rax, r14                     ; tile_x
    add rax, r9                      ; + current x
    cmp rax, r10                     ; Sprawdü czy nie przekraczamy width
    jge fill_next_y
    
    ; Oblicz finalny adres piksela
    lea rcx, [rsi]                   ; Za≥aduj bazowy adres obrazu
    add rcx, rbx                     ; Dodaj offset wiersza
    lea rcx, [rcx + rax*4]          ; Dodaj offset piksela (x * 4 bajty na piksel)

    ; Sprawdü czy adres jest w zakresie bufora
    cmp rcx, rsi
    jl fill_x_next                   ; PomiÒ jeúli adres jest przed buforem
    
    mov rax, rdi                     ; stride
    mul r11                          ; * height (ca≥kowity rozmiar bufora)
    add rax, rsi                     ; KoÒcowy adres bufora
    cmp rcx, rax
    jge fill_x_next                  ; PomiÒ jeúli adres jest za buforem

    ; Zapisz úrednie wartoúci do piksela
    movd eax, xmm1                   ; Weü B
    and eax, 0FFh                    ; Zostaw tylko najm≥odszy bajt
    mov byte ptr [rcx], al           ; B

    movd eax, xmm2                   ; Weü G
    and eax, 0FFh                    ; Zostaw tylko najm≥odszy bajt
    mov byte ptr [rcx + 1], al       ; G

    movd eax, xmm3                   ; Weü R
    and eax, 0FFh                    ; Zostaw tylko najm≥odszy bajt
    mov byte ptr [rcx + 2], al       ; R

    mov byte ptr [rcx + 3], 0FFh     ; Alpha = 255

fill_x_next:
    inc r9                           ; x++
    jmp fill_x_loop

fill_next_y:
    inc r8                           ; y++
    jmp fill_y_loop

next_tile_x:
    add r14, r12                    ; tile_x += tileSize
    jmp tile_x_loop

next_tile_row:
    add r13, r12            ; tile_y += tileSize
    jmp tile_y_loop

done:
    pop r15
    pop r14
    pop r13
    pop r12
    pop rbx
    pop rdi
    pop rsi
    mov rsp, rbp
    pop rbp
    ret

ApplyMosaicASM endp
end