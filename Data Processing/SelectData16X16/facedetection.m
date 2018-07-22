%facedetector  Lasthope
clear all; clc;
%load('fileandellipse.mat');
load('fileellipseangle.mat')
s=-1;
ang=60*pi/180;
m=16;
Data=uint8(zeros(120000,m*m*3));
idx=1;
Et=[1 1.5 0.4]; 
fac=[1 0.95 0.9 0.85 0.8 0.75];
for i=1:21000
if (size(c{i}{2},1)==1)
if (abs(c{i}{3}(3))<ang)
    
    data=c{i}{2};
if (2*max(data(3:4))>64)

f=selectFolderAFWL(c{i}{1});
% if (size(f,3)==3)  
%     f=rgb2gray(f);
% end
if (size(f,3)>1)
m=16;

for j=1:length(fac)
lan=randi([1 length(Et)],1);
x=floor(data(1));
y=floor(data(2)+data(3)*(1-fac(j))*2/3);
r=floor(max(data(3:4))*fac(j));                                  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% 
T=floor(max(y-r,1));
B=min(y+r,size(f,1));
L=max(x-r,1);
R=min(x+r,size(f,2));
ff=f(T:B,L:R,:);
Data(idx,:)=  reshape( rot90(ExpoTrans(imresize(ff,16*[1 1]),Et(lan))),1,[]);
idx=idx+1;
end

if (mod(i,500)==0)
i
end
end
end
end
end
end
Data=Data(1:idx,:);
Data=uint8(single(Data)+ floor(20*(rand(size(Data))-0.5)));
    


