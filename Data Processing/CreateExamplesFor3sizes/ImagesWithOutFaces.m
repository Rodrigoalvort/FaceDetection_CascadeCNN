%% generar imagenes sin caras :D

clear all; clc;
%load('fileandellipse.mat');
load('fileellipseangle.mat')
path='E:\Bases de datos\AFWL\aflw\data\flickr\ImagenesSinRostros2\';
s=-1;

for i=5001:15000
data=c{i}{2};
if (size(c{i}{2},1)==1)
if (2*max(data(3:4))>32)
f=selectFolderAFWL(c{i}{1});
% if (size(f,3)==3)  
%     f=rgb2gray(f);
% end

if (size(f,3)>1)
m=16;
x=floor(data(1));y=floor(data(2)); r=floor(max(data(3:4))*1.1);                                  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% 
T=max(y-r,1); B=min(y+r,size(f,1)); L=max(x-r,1); R=min(x+r,size(f,2));


ff=f(T:B,L:R,:);%imshow(ff)
f(T:B,L:R,1)=mean(mean(ff(:,:,1)));
f(T:B,L:R,2)=mean(mean(ff(:,:,2)));
f(T:B,L:R,3)=mean(mean(ff(:,:,3)));
imwrite(f,strcat(path,num2str(i),'.jpg'));
% imshow(f);
% menu('','');

if (mod(i,500)==0)
i
end
end
end
end
end