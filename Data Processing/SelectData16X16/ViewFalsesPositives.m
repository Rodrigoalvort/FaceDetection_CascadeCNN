clear all; clc;

load('E:\Bases de datos\BasesDeDatosMatlab\FaceDetection12_16_32\tptn16.mat')
input=output'; clear Data;
%%load('E:\DeepLearning\cntk\Examples\Facedetection12X12X3-Weak60Angle\OutputNodes.z.mat');
load('E:\DeepLearning\cntk\Examples\FaceDetection16X16X3_VF\OutputNodes.z.mat');
    
thd=0.505;
output=output(:,2)';

%% falsos positivos
m=16;
error=and(not(target'),output>=thd);
fp=rot90(reshape(input(:,error),m,m,3,[]),-1);
idx=1;
image=uint8([]);
for i=1:50
for j=1:50
image(m*(i-1)+1:m*i,m*(j-1)+1:m*j,:)=uint8(fp(:,:,:,idx));
idx=idx+1;
end
end
imshow((image));
%% falsos negativos
image=uint8([]);
m=16;
error=and(logical(target'),output<thd);
fp=rot90(reshape(input(:,error),m,m,3,[]),-1);
idx=1;

for i=1:50
for j=1:50
image(m*(i-1)+1:m*i,m*(j-1)+1:m*j,:)=uint8(fp(:,:,:,idx));
idx=idx+1;
end
end
imshow((image));


%% Falsos positivos generados
clear all; clc;
load('E:\Bases de datos\BasesDeDatosMatlab\FaceDetection12_16_32\falses12.mat')
output=reshape(output',16,16,3,[]);
m=16;
idx=1;
for k=1:40
for i=1:75
for j=1:100
image(m*(i-1)+1:m*i,m*(j-1)+1:m*j,:)=uint8(output(:,:,:,idx));
idx=idx+1;
end
end
imshow((image));
menu('','');
end