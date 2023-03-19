.data
constR dd 150
constG dd 100
constB dd 100
.code
MyProc1 proc
pmovzxbd xmm0, [rcx] ;przepisanie tego co znajduje si� w pami�ci do rejestru xmm0
movd eax, xmm0		 ;zapisanie rejestru xmm0 do rejestru eax
cmp eax, constR		 ;por�wnanie eax do sta�ej constR r�wnej 150
ja w				 ;je�li warto�� eax wi�ksza od 150 skocz do w
psrldq xmm0, 4		 ;przesuni�cie o 4 bity w prawo rejestru xmm0
movd eax, xmm0		 ;zapisanie warto�ci rejestru xmm0 do rejestru eax
cmp eax, constG		 ;por�wnanie eax do sta�ej constG r�wnej 100
jb w				 ;je�li warto�� eax mniejsza od 100 skocz do w
psrldq xmm0, 4		 ;przesuni�cie o 4 bity w prawo rejestru xmm0
movd eax, xmm0		 ;zapisanie warto�ci rejestru xmm0 do rejestru eax
cmp eax, constB		 ;por�wnanie eax do sta�ej constB r�wnej 100
ja w				 ;je�li warto�� eax wi�ksza od 100 skocz do w
mov eax, 255		 ;zapisanie na rejestr eax warto�ci 255
mov [rcx], eax		 ;wpisanie warto�ci eax na pierwsze miejsce w tablicy w pami�ci
mov [rcx + 1], eax	 ;wpisanie warto�ci eax na drugie miejsce w tablicy w pami�ci
mov [rcx + 2], eax	 ;wpisanie warto�ci eax na trzecie miejsce w tablicy w pami�ci

w:
ret
MyProc1 endp
end