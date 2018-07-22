clear all; clc;close all;
load('E:\Bases de datos\CACD2000\celebrity2000_meta.mat')
load('facedetection.mat');
 % orden para usar insertobjectannotation
%[width height num y x]  % orden como esta data
%% fueron las detecciones obtenidas  que se demmoro como 2 dias jajaajajaj u.u
p=~isnan(data(:,1));  
data=data(p,:);
names=names(p);

%% se mejora el enventanado obtenido  por ser tan peuqeño
data(:,4)=data(:,4)-0.10*data(:,2);
data(:,5)=data(:,5)-(0.10+0.125)*data(:,2);

data(:,1:2)=data(:,1:2)*1.2;
data(:,4)=data(:,4)*250/480;%y
data(:,5)=data(:,5)*250/640;%x
data(:,1)=data(:,1)*250/480;%width
data(:,2)=data(:,2)*250/480;%height


datos=uint8(zeros(size(data,1),24*24*3));
%% se inicia a escribir el txt
for i=1:size(data,1)
try
d=floor(data(i,:));
f=imread(strcat('E:\Bases de datos\CACD2000\CACD2000\',names{i}));
f=f(d(4):d(4)+d(1),d(5):d(5)+d(1),:);
%%E:\ucireaderdataset\Imagesfacerecognition

datos(i,:)=reshape(rot90(imresize(f,[24 24])),1,[]);
%imshow();
if (mod(i,2000)==0)
    100*i/size(data,1)
end

catch 
end
end
 
