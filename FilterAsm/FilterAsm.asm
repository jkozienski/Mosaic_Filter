;;ten kod wczytuje pixele do xmm

.data
    align 16
    zero_vector dd 0, 0, 0, 0
.code
ApplyMosaicASM proc
    ; Parametry:
    ; RCX - wska�nik do danych obrazu (sourcePtr)
    ; RDX - stride (szeroko�� linii w bajtach)
    ; R8  - szeroko�� obrazu (width)
    ; R9  - wysoko�� obrazu (height)
    ; [RSP+40] - rozmiar kafelka (tileSize)

    ; Zachowanie rejestr�w
    push rbp
    mov rbp, rsp
    push rsi
    push rdi
    push rbx
    push r12
    push r13
    push r14
    push r15

    ; Zachowanie argument�w w bezpiecznych rejestrach
    mov rsi, rcx            ; sourcePtr
    mov rdi, rdx            ; stride
    mov r10, r8             ; width
    mov r11, r9             ; height
    mov r12, [rbp+48]       ; tileSize

    ; Iteracja po kafelkach
    xor r13, r13            ; tile_y = 0
tile_y_loop:
    cmp r13, r11            ; Sprawd� czy tile_y < height
    jge done

    xor r14, r14            ; tile_x = 0
tile_x_loop:
    cmp r14, r10            ; Sprawd� czy tile_x < width
    jge next_tile_row

    ; Oblicz rozmiar aktualnego kafelka
    mov r15, r12            ; current_tile_width = tileSize
    mov rbx, r10
    sub rbx, r14            ; remaining_width = width - tile_x
    cmp r15, rbx
    jle use_current_width
    mov r15, rbx            ; U�yj remaining_width je�li jest mniejsze
use_current_width:

    ; Inicjalizacja sum kolor�w dla kafelka
    pxor xmm1, xmm1        ; Suma B (dolne 32 bity)
    pxor xmm2, xmm2        ; Suma G (dolne 32 bity)
    pxor xmm3, xmm3        ; Suma R (dolne 32 bity)
    xor r8d, r8d           ; Licznik pikseli

    ; Iteracja po pikselach w kafelku
    xor r8, r8              ; y = 0
pixel_y_loop:
    cmp r8, r15             ; Sprawd� czy y < current_tile_width
    jge calc_average

    ; Oblicz adres pocz�tku wiersza
    mov rax, r13
    add rax, r8             ; y + tile_y
    mul rdi                 ; * stride
    lea rax, [rsi + rax]    ; adres pocz�tku wiersza

    xor r9, r9              ; x = 0
pixel_x_loop:
    cmp r9, r15             ; Sprawd� czy x < current_tile_width
    jge next_pixel_y

    ; Oblicz adres piksela
    mov rcx, r14
    add rcx, r9             ; x + tile_x
    lea rcx, [rax + rcx*4]  ; adres piksela

    ; Wczytaj piksel do xmm0 (BGRA)
    movd xmm0, dword ptr [rcx]
    
    ; Rozpakuj bajty do s��w
    punpcklbw xmm0, xmm4    ; Rozszerza bajty do s��w (16-bit)
    punpcklwd xmm0, xmm4    ; Rozszerza s�owa do double-word�w (32-bit)

    ; Dodaj sk�adowe do sum
    paddd xmm1, xmm0        ; Dodaje B
    psrld xmm0, 8           ; Przesuwa do G
    paddd xmm2, xmm0        ; Dodaje G
    psrld xmm0, 8           ; Przesuwa do R
    paddd xmm3, xmm0        ; Dodaje R

    inc r8d                 ; Zwi�ksz licznik pikseli
    inc r9                  ; x++
    jmp pixel_x_loop

next_pixel_y:
    inc r8                  ; y++
    jmp pixel_y_loop

calc_average:
    ; Tu b�dzie kod obliczaj�cy �rednie warto�ci
    ; i wype�niaj�cy kafelek

next_tile_x:
    add r14, r12            ; tile_x += tileSize
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