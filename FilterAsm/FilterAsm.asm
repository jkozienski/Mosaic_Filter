.code
ApplyRedFilter proc
    ; RCX - wskaŸnik do danych obrazu
    ; RDX - szerokoœæ obrazu
    ; R8 - wysokoœæ obrazu

    push rbp
    mov rbp, rsp

    ; Zachowaj wskaŸnik do danych obrazu
    mov r10, rcx    ; r10 = wskaŸnik do danych
    mov r11, rdx    ; r11 = szerokoœæ
    mov r12, r8     ; r12 = wysokoœæ

    ; Oblicz ca³kowit¹ liczbê pikseli
    mov rax, r11
    mul r12         ; rax = szerokoœæ * wysokoœæ

    ; Iteruj po wszystkich pikselach
    xor rcx, rcx    ; licznik pikseli
pixelLoop:
    cmp rcx, rax
    jge done

    ; Oblicz offset dla aktualnego piksela (4 bajty na piksel - BGRA)
    mov r13, rcx
    shl r13, 2      ; r13 = rcx * 4

    ; Wczytaj piksel
    mov edx, dword ptr [r10 + r13]

    ; Zachowaj czerwony kana³ (jest w trzecim bajcie)
    and edx, 0FF0000h    ; zachowaj tylko czerwony sk³adnik
    or edx, 0FF000000h   ; ustaw pe³n¹ nieprzezroczystoœæ (alpha)

    ; Zapisz zmodyfikowany piksel
    mov dword ptr [r10 + r13], edx

    inc rcx
    jmp pixelLoop

done:
    mov rsp, rbp
    pop rbp
    ret
ApplyRedFilter endp

end