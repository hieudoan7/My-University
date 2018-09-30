#include <stdio.h>
#include <stdlib.h>
#include <fcntl.h>
#include <errno.h>

#define DEVICE "/dev/1612198_device"
#define BUFFER_LENGTH 4


int main(){
	int  fd; //file description
	char read_buf[BUFFER_LENGTH];
	fd = open(DEVICE,O_RDWR);  //open for reading and writing
	if (fd==-1){
		printf("file %s either does not exist or has been locked by another process\n",DEVICE);
		exit(-1);
	}
	int ret=read(fd,read_buf,BUFFER_LENGTH);
	if(ret<0){
		perror("Failed to write the message to the device");
		return errno;
	}
	int randNumber = *((int *)read_buf); //casting 
	printf("The random Number: %d\n",randNumber);
	close(fd);
	return 0;
}