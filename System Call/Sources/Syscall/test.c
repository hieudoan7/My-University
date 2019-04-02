#include <stdio.h>
#include <linux/kernel.h>
#include <sys/syscall.h>
#include <unistd.h>
#include <string.h>
int main(){
	printf("Choose the following options:\n1. pnametoid \n2. pidtoname\n");
	int choose;
	printf("You choose: ");
	scanf("%d", &choose);
	if(choose == 1){
		char name[100];
		printf("Enter your name: ");
		scanf("%s", name);
		strtok(name, "\n");
		int pid = syscall(350, name);
		printf("System call return %d\n", pid); 	
	}
	else if(choose == 2){
		char bufName[100];
		int pid;
		printf("Enter your pid: ");
		scanf("%d", &pid);
		int result_len = syscall(351, pid, bufName, 100);
		printf("System call returned %d\n", result_len);
		if(result_len > 0){
			printf("Your process name: %s\n", bufName);
		}
		else{
			printf("Cannot find the process with pid = %d\n", pid);
		}
	}
       	return 0;
}
