import os
from flask import Flask, request, Response
from flask import render_template
from darknet import *
import cv2


app = Flask(__name__)

net, meta = yolo()

UPLOAD_FOLDER = '/home/hieudoan7/darknet/static'
# UPLOAD_FOLDER = os.path.basename('uploads')

app.config['UPLOAD_FOLDER'] = UPLOAD_FOLDER

@app.route('/')
def hello_world():
    return render_template('index.html',name="dog.jpg")

@app.route('/upload', methods=['POST'])
def upload():
    file = request.files['photo']
    f = os.path.join(app.config['UPLOAD_FOLDER'], file.filename)
    
    # # add your custom code to check that the uploaded file is a valid image and not a malicious file (out-of-scope for this post)
    file.save(f)
    f_name=file.filename.encode('ascii','ignore')

    r = detect(net, meta, f)
    bounding_box(r, f_name)
    print(r)
    return render_template('index.html', name=f_name)
    
def bounding_box(res, f):
    i = 0
    path = os.path.join(UPLOAD_FOLDER,f)
    img = cv2.imread(path,cv2.IMREAD_COLOR)
    colors = [(0,0,255),(0,255,0),(255,0,0),(0,255,255),(255,255,0)] #BGR
    while i<len(res):
        label=res[i][0]
        confi = res[i][1]*1000//1/10
        
        box_color = colors[i%5]
        center_x=int(res[i][2][0])
        center_y=int(res[i][2][1])
        width = int(res[i][2][2])
        height = int(res[i][2][3])

        TL_x = int(center_x - width/2) #Upper Left corner X coord
        TL_y = int(center_y - height/2) #Upper left Y
        BR_x = int(center_x + width/2)
        BR_y = int(center_y + height/2)

        #write bounding box to image
        cv2.rectangle(img,(TL_x,TL_y),(BR_x,BR_y),box_color,2)
        #put label on bounding box
        font = cv2.FONT_HERSHEY_SIMPLEX
        label=label+' '+str(confi)+'%'
        print(label)
        sz = len(label)*10
        if TL_y-17 >= 0:
            cv2.rectangle(img,(TL_x,TL_y-17),(TL_x+sz,TL_y),box_color,-1)
            cv2.rectangle(img,(TL_x,TL_y-17),(TL_x+sz,TL_y),box_color,2)
            cv2.putText(img,label,(TL_x,TL_y-5),font,0.5,(0,0,0),1,cv2.LINE_AA)

        else:
            cv2.rectangle(img,(TL_x,TL_y+17),(TL_x+sz,TL_y),box_color,-1)
            cv2.rectangle(img,(TL_x,TL_y+17),(TL_x+sz,TL_y),box_color,2)
            cv2.putText(img,label,(TL_x+2,TL_y+14),font,0.5,(0,0,0),1,cv2.LINE_AA)
 
        i=i+1
    f="predict_"+f
    new_path = os.path.join(UPLOAD_FOLDER,f)
    cv2.imwrite(new_path,img) #wait until all the objects are marked and then write out.
    return "OK"

if __name__ == "__main__":
    app.run(debug=True)
