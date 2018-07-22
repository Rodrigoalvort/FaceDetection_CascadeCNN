% exponencial 
function Y=ExpoTrans(image,y);
image=double(image);
c=(255/(255^y));
Y=uint8(c*image.^y);

