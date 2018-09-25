#include <stdio.h>
#include "Header.h"
#include <Windows.h>



void main()
{
	system("mode 120, 33");
	FILE *f = fopen("lena.bmp", "rb");
	if (isBmpFile(f)) printf("La file bmp! \n");
	else printf("Khong la file bmp! \n");
	BmpHeader Header;
	readBmpHeader(f, Header);
	printBmpHeader(Header);
	BmpDib Dib;
	readBmpDib(f, Dib);
	printBmpDib(Dib);
	PixelArray data;
	readBmpPixelArray(f, Header, Dib, data);
	for (int i = 0; i < data.rowCount;i++)
	for (int j = 0; j < data.columnCount; j++){
		if (data.pixels[i][j].red == 255 && data.pixels[i][j].green == 0 && data.pixels[i][j].blue == 0)
		{
			data.pixels[i][j].red = 255;
			data.pixels[i][j].green = 0;
			data.pixels[i][j].blue = 255;
		}
	}
	
	fclose(f);
	drawBmp(Dib, data);
	char *filePath = "BmpOut.bmp";
	writeBMP(filePath, Header, Dib, data);
	releaseBmpPixelArray(data);
	getchar();

}