.code
ApplyRedFilter proc
    ; RCX - wska�nik do danych obrazu
    ; RDX - szeroko�� obrazu
    ; R8 - wysoko�� obrazu

    push rbp
    mov rbp, rsp

    ; Zachowaj wska�nik do danych obrazu
    mov r10, rcx    ; r10 = wska�nik do danych
    mov r11, rdx    ; r11 = szeroko��
    mov r12, r8     ; r12 = wysoko��

    ; Oblicz ca�kowit� liczb� pikseli
    mov rax, r11
    mul r12         ; rax = szeroko�� * wysoko��

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

    ; Zachowaj czerwony kana� (jest w trzecim bajcie)
    and edx, 0FF0000h    ; zachowaj tylko czerwony sk�adnik
    or edx, 0FF000000h   ; ustaw pe�n� nieprzezroczysto�� (alpha)

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