#include <linux/syscalls.h>
#include <linux/kernel.h>
#include <linux/sched.h>
#include <linux/init.h>
#include <linux/string.h>
#include "pnametoid.h"
asmlinkage int sys_pnametoid(char* name){
    struct task_struct *task;
    for_each_process(task){
        if(strcmp(task->comm, name) == 0){
            return (int)task_pid_nr(task);
        }
    }
    return -1;
}
