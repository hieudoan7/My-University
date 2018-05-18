#pragma once
#include <stdint.h>

#pragma pack(1)
struct BmpSignature
{
	unsigned char data[2];
};

struct BmpHeader
{
	BmpSignature signature;
	uint32_t fileSize;
	uint16_t reserved1;
	uint16_t reserved2;
	uint32_t dataOffset;
};

struct BmpDib
{
	uint32_t dibSize;
	int32_t	 imageWidth;
	int32_t  imageHeight;
	uint16_t colorPlaneCount;
	uint16_t pixelSize;
	uint32_t compressMethod;
	uint32_t bitmapByteCount;
	int32_t  horizontalResolution;
	int32_t  verticalResolution;
	uint32_t colorCount;
	uint32_t importantColorCount;
};

struct Color
{
	unsigned char blue;
	unsigned char green;
	unsigned char red;
};

struct ColorTable
{
	Color	 *colors;
	uint32_t length;
};

struct PixelArray
{
	Color	 **pixels;
	uint32_t rowCount;
	uint32_t columnCount;
};

bool isBmpFile(FILE *f);

void readBmpHeader(FILE *f, BmpHeader &header);
void printBmpHeader(BmpHeader header);

void readBmpDib(FILE *f, BmpDib &dib);
void printBmpDib(BmpDib dib);

void readBmpPixelArray(FILE *f, BmpHeader header, BmpDib dib, PixelArray &data);
void scanBmpPixelLine(FILE *f, Color *&line, uint32_t length);
void skipBmpPadding(FILE *f, char count);

void drawBmp(BmpDib dib, PixelArray data);
void releaseBmpPixelArray(PixelArray data);
void writeBMP(char* filePath, BmpHeader header, BmpDib Dib, PixelArray data);
void printBmpPixelLine(FILE *&f, Color *&line, uint32_t length);
void printBmpPadding(FILE *&f, char count);