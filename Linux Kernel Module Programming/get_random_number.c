#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>

#define DEVICE "/dev/1612198_device"

int main(){
	int  fd; //file description
	char read_buf[100];
	fd = open(DEVICE,O_RDWR);  //open for reading and writing
	if (fd==-1){
		printf("file %s either does not exist or has been locked by another process\n",DEVICE);
		exit(-1);
	}
	int randNumber=read(fd,read_buf,sizeof(read_buf));
	printf("The random Number: %d\n",randNumber);
	close(fd);

	return 0;
}