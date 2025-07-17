import firebase_admin
from firebase_admin import credentials, storage, db
import random
import string
from PIL import Image, ImageWin, ImageFont, ImageDraw
import os
import win32print
import win32ui
import time
from datetime import datetime
import qrcode

url_base = "myurl"

random_key = ""

qrsize = 150

x_size, y_size = 650, 364
default_x_offset, default_y_offset = 270, 70
gap = 7

file_path = 'output.png'

txt_file = "index.txt"

qr_path = "qr.png"

paper_width_inches = 4
paper_height_inches = 6
dpi = 600  # 프린터의 DPI 설정 *************이 부분 조절해서 실제 크기 맞출 것!!!!

qr_x = 950
qr_y = 50

# Firebase 초기화
cred = credentials.Certificate("auth.json")
firebase_admin.initialize_app(cred, {
    'storageBucket': 'my.appspot.com',
    'databaseURL': 'https://my-default-rtdb.firebaseio.com/'
})
 
def print_image(image_path, copies):
    # 기본 프린터를 가져옵니다.
    printer_name = win32print.GetDefaultPrinter()

    # 이미지를 열고 프린터에 맞게 조정합니다.
    image = Image.open(image_path)
    image = image.convert("RGB")  # RGB 모드로 변환

    # 프린터의 용지 크기 설정
    

    # 용지 크기를 픽셀 단위로 변환
    paper_width_pixels = int(paper_width_inches * dpi)
    paper_height_pixels = int(paper_height_inches * dpi)

    # 이미지 크기를 용지 크기에 맞게 조정
    image = image.resize((paper_width_pixels, paper_height_pixels), Image.LANCZOS)

    for _ in range(copies):
        # 프린터 DC를 엽니다.
        hdc = win32ui.CreateDC()
        hdc.CreatePrinterDC(printer_name)
        hdc.StartDoc('Print Job')
        hdc.StartPage()

        # 이미지 정보를 프린터에 전송합니다.
        dib = ImageWin.Dib(image)
        dib.draw(hdc.GetHandleOutput(), (0, 0, paper_width_pixels, paper_height_pixels))

        # 페이지와 문서를 종료합니다.
        hdc.EndPage()
        hdc.EndDoc()
        hdc.DeleteDC()
        
def upload_image_to_firebase(image_path, file_name):
    # Firebase Storage 버킷 가져오기
    bucket = storage.bucket()
    # 업로드할 파일 이름 설정
    blob = bucket.blob(file_name + ".png")
    # 파일 업로드
    blob.upload_from_filename(image_path)
    # 파일의 공개 URL 가져오기
    blob.make_public()
    print('upload img SUCCESS!')
    return blob.public_url

def generate_random_key():
    print('Gen Random keys...')
    return ''.join(random.choices(string.digits, k=10))
    

def check_key_exists(key):
    ref = db.reference(f'/images/{key}')
    print('Check keys...')
    return ref.get() is not None

def save_image_url_to_firebase(image_url):
    while True:
        random_key = generate_random_key()
        if not check_key_exists(random_key):
            ref = db.reference(f'/images/{random_key}')
            ref.set({
                'url': image_url
            })
            print(f"Image URL saved with key: {random_key}")
            print(url_base + "?key=" + random_key)
            return random_key

def qrgen(url, save_path):
    img = qrcode.make(url)
    img.save(save_path)
    print("Gen QR SUCCESS")

def loadfont(fontsize=50):
	# ttf파일의 경로를 지정합니다.
	ttf = 'font.ttf'
	return ImageFont.truetype(font=ttf, size=fontsize)
    #size : in pixels



 
print_name = win32print.GetDefaultPrinter()
print("Print dev : " + print_name)

if os.path.exists(file_path):
    os.remove(file_path)
    
foreground = [""] * 4
resize_fore = [""] * 4
line = []

#이미지 파일 오픈 
f = open(txt_file, 'r')

line_raw = f.read()
line = line_raw.split("\n")
for i in range(0,6):
    line[i] = line[i].strip()

print(line[5])

background = Image.open(line[0])
for i in range(0,4):
    foreground[i] = Image.open(line[i+1])


#합성할 배경 이미지를 위의 파일 사이즈로 resize
for i in range(0,4):
    resize_fore[i] = foreground[i].resize((x_size, y_size))

#이미지 합성 
for i in range(0,4):
    background.paste(resize_fore[i], (default_x_offset, default_y_offset + (y_size+gap)*i))

current_datetime = datetime.now()

importdate = current_datetime.strftime("%Y-%m-%d %H:%M:%S")

titlefontObj = loadfont()
title_text = importdate
out_img = ImageDraw.Draw(background)
out_img.text(xy=(50,1700), text=title_text, fill=(20, 20, 20), font=titlefontObj)

#합성한 이미지 파일 보여주기 
background.save(file_path)
f.close()

print("IMG SEQ DONE!")


print("Wait Until IMG Gen...")
time.sleep(0.5)


current_datetime = datetime.now()
datetime_string = current_datetime.strftime("%Y%m%d%H%M%S")



image_path = file_path
image_url = upload_image_to_firebase(image_path, datetime_string)
random_key = save_image_url_to_firebase(image_url)
print(url_base + "?key=" + random_key)
qrgen(url_base + "?key=" + random_key, qr_path)
print("Wait Until QR Gen...")
time.sleep(0.5)

bg = Image.open("output.png")
qr = Image.open(qr_path)
qr = qr.resize((qrsize, qrsize))
bg.paste(qr, (qr_x, qr_y))
bg.save(file_path)

time.sleep(0.5)
print("Wait Final Gen...")

image_url = upload_image_to_firebase(image_path, datetime_string)
print("Start Printing SEQ...")
print("printing img... page : " + list[5])
#print_image('output.png', int(list[5]))  #************* 나중에 다시 킬 것!
