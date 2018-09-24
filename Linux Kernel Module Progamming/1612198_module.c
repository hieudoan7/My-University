#include <linux/module.h>
#include <linux/kernel.h>
#include <linux/fs.h>  //file_operations structure- which of course allows use to open/close, read/write to device
#include <linux/cdev.h> //char device; make cdev available
#include <linux/semaphore.h> //used to access semaphore: synchronization behaviors
#include <asm/uaccess.h> //copy_to_user; copy_from_user
#include <linux/uaccess.h>
#include <linux/random.h>  //get_random_bytes


//To later register our device we need a cdev object and some other variables

struct cdev *mcdev;  //m stands 'my'
int major_number;  //will store our major number-extracted from dev_t using marco - mknod /directory/file c major minor
int ret;           //will be used to hold return values of functions; this is because the kernel stack is very small

dev_t dev_num;

#define DEVICE_NAME	 "1612198_device"
 

//called when user wants to get information from the device
ssize_t device_read(struct file* filp, char* bufStoreData,size_t bufCount,loff_t* curOffset){
	int randomNumber;
	get_random_bytes(&randomNumber,sizeof(randomNumber));
	return randomNumber;
}



//Tell the kernel which functions to call when user operates on our device file
struct file_operations fops={
	.owner = THIS_MODULE,  	//prevent unloadind of this module when operations are in use
	.read = device_read		//points to the method to call when reading from the device
};

 
 //Register your device with system: 2 step
static int driver_entry(void){
	//step (1) use dynamic allocation to assign our device
	// a major number-- alloc_chrdev_region(dev_t*, unit fminor,unit count, char* name)
	ret = alloc_chrdev_region(&dev_num,0,1,DEVICE_NAME);
	if (ret<0) {
		printk(KERN_ALERT "1612198: failed to allcate a major number");
		return ret;
	}
	major_number= MAJOR(dev_num); //extracts the major number and store in our variable (MACRO)
	printk(KERN_INFO "1612198: major number is %d",major_number);
	printk(KERN_INFO "\tuse \"mknod /dev/%s c %d 0\" for device file",DEVICE_NAME,major_number); //dmesg
	//step(2)
	mcdev=cdev_alloc(); //create our cdev structure, initialized our cdev
	mcdev->ops=&fops; //struct file_operations
	mcdev->owner=THIS_MODULE;
	//now that we create cdev, we have to add it to the kernel
	//int cdev_add(struct cdev* dev, dev_t num, unsigned int count);
	ret = cdev_add(mcdev,dev_num,1);
	if(ret<0) { //always check errors
		printk(KERN_ALERT "1612198: unable to add cdev to kernel");
		return ret;
	}
	return 0;
}

//unregister everything in reverse order
static void driver_exit(void){
	cdev_del(mcdev);
	unregister_chrdev_region(dev_num,1);
	printk(KERN_ALERT "1612198_module: unloaded module");
}
//inform the kernel where to start and stop with our module/driver
module_init(driver_entry);
module_exit(driver_exit);