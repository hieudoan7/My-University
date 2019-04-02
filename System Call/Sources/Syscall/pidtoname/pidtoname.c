#include <linux/syscalls.h>
#include <linux/kernel.h>
#include <asm/uaccess.h>
#include <linux/init.h>
#include <linux/tty.h>
#include <linux/string.h>
#include <linux/pid.h>
#include <linux/pid_namespace.h>
#include "pidtoname.h"

asmlinkage int sys_pidtoname(int pid, char* buf, int len){
    struct task_struct *task;
    struct pid *pid_struct;
    int len_process_name=0;
    pid_struct = find_get_pid(pid);
    task = pid_task(pid_struct,PIDTYPE_PID);
    len_process_name = sprintf(buf,"%s",task->comm);
    if(len>len_process_name)
        return 0;
    if(len<=len_process_name)
        return len_process_name;
    return -1;
}
