Prepare
# Syscall and Hook
Write two custom syscall pidtoname and pnametoid. Hook open and write syscall.
### Prerequisites
Install prerequisites:
```
	sudo apt-get install build-essential libncurses5-dev bison flex libssl-dev libelf-dev
```
## Installing
### Get kernel source

Download linux kernel 3.16.61
```
	wget https://cdn.kernel.org/pub/linux/kernel/v3.x/linux-3.16.61.tar.xz
```
Extract
```
	sudo tar -xvf linux-3.16.61.tar.xz -C /usr/src/
```
Change directory ::
```
	cd /usr/src/linux-3.16.61/
```
### Create syscall

``pnametoid`` system call, ''pidtoname'' is the same:

Create ``pnametoid`` directory to kernel directory

Edit kernel Makefile, append ``pnametoid/`` in the end of ``core-y`` line
```
	core-y += kernel/ mm/ fs/ ipc/ security/ crypto/ block/ pnametoid/
```
Change directory to syscall table
```	
	cd /usr/src/linux-3.16.61/
	cd arch/x86/syscalls/
```
Edit ``syscall_64.tbl``, add this line in the end of file
```
	350 common pnametoid sys_pnametoid
```
Change directory to syscall header
```
	cd /usr/src/linux-3.16.61/
	cd include/linux/
```
Add the prototype of syscall in the end of file, before ``#endif``

## Build Kernel

### First time

Run menuconfig
```
	cd /usr/src/linux-3.16.61/
	sudo make menuconfig
```	
Compile
```	
	sudo make -j2
```
Install 
```	
	sudo make modules_install install
```
### Other time

Only need 
```
	sudo make -j2
	sudo make install
```
## Hook

### Install the module
Before install, delete the dmesg

```
dmesg -C
```

After that, install the module

```
sudo insmod test.ko
```
### Check the dmesg

Test open file

```
dmesg | grep 'gnome-terminal'
```

Test write file

```
dmesg | grep test
