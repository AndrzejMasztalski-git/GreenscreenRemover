.data
constR dd 150
constG dd 100
constB dd 100
.code
MyProc1 proc
pmovzxbd xmm0, [rcx] ;przepisanie tego co znajduje siê w pamiêci do rejestru xmm0
movd eax, xmm0		 ;zapisanie rejestru xmm0 do rejestru eax
cmp eax, constR		 ;porównanie eax do sta³ej constR równej 150
ja w				 ;jeœli wartoœæ eax wiêksza od 150 skocz do w
psrldq xmm0, 4		 ;przesuniêcie o 4 bity w prawo rejestru xmm0
movd eax, xmm0		 ;zapisanie wartoœci rejestru xmm0 do rejestru eax
cmp eax, constG		 ;porównanie eax do sta³ej constG równej 100
jb w				 ;jeœli wartoœæ eax mniejsza od 100 skocz do w
psrldq xmm0, 4		 ;przesuniêcie o 4 bity w prawo rejestru xmm0
movd eax, xmm0		 ;zapisanie wartoœci rejestru xmm0 do rejestru eax
cmp eax, constB		 ;porównanie eax do sta³ej constB równej 100
ja w				 ;jeœli wartoœæ eax wiêksza od 100 skocz do w
mov eax, 255		 ;zapisanie na rejestr eax wartoœci 255
mov [rcx], eax		 ;wpisanie wartoœci eax na pierwsze miejsce w tablicy w pamiêci
mov [rcx + 1], eax	 ;wpisanie wartoœci eax na drugie miejsce w tablicy w pamiêci
mov [rcx + 2], eax	 ;wpisanie wartoœci eax na trzecie miejsce w tablicy w pamiêci

w:
ret
MyProc1 endp
end