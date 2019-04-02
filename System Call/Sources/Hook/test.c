#include <fcntl.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <stdio.h>
#include <linux/kernel.h>
#include <sys/syscall.h>
#include <unistd.h>
#include <string.h>
int main(){
int fd = open("in.txt",O_WRONLY | O_CREAT | O_APPEND);
printf("FILE DESCRIPTION: %d\n",fd);
if(write(fd, "He Dieu Hanh\n", 13) == 13) {
    printf("WRITE SUCCESSFULLY\n");
}
else{
    printf("WRITE FAILURE\n");
}
return 0;
}
