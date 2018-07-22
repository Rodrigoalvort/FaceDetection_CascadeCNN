%facedetector  Lasthope
clear all; clc;
%load('fileandellipse.mat');
load('fileellipseangle.mat')
s=-1;
ang=80*pi/180;
m=24;
Data=uint8(zeros(120000,m*m*3));
idx=1;
Et=[1 1.5 0.4]; 
fac=[1 0.975 0.95 0.925 0.9 0.875];
siz=32*1.5.^[0:6];
for i=1:20000

for j=1:size(c{i}{2},1)
if (abs(c{i}{3}(j,3))<ang)
    
    data=c{i}{2}(j,:);
if (2*max(data(3:4))>64)

f=selectFolderAFWL(c{i}{1});
% if (size(f,3)==3)  
%     f=rgb2gray(f);
% end
if (size(f,3)==1)
f(:,:,2)=f;
f(:,:,3)=f(:,:,2);
end
    
for fa=1:length(fac)
lan=randi([1 length(Et)],1);
flipi=randi([0 1],1);

x=floor(data(1));
y=floor(data(2)+data(3)*(1-fac(fa))*2/3);
r=floor(max(data(3:4))*fac(fa));                                  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% 
T=floor(max(y-r,1));
B=min(y+r,size(f,1));
L=max(x-r,1);
R=min(x+r,size(f,2));
%ff=ExpoTrans(imresize(f(T:B,L:R,:),m*[1 1]),Et(lan));subplot(2,6,fa);imshow(ff);
ya=floor(T:(B-T+1)/m:B);
xa=floor(L:(R-L+1)/m:R);
ff=ExpoTrans((f(ya,xa,:)),Et(lan));
%subplot(2,6,6+fa);imshow(ff);

if flipi==1
    ff=fliplr(ff);
end
Data(idx,:)=  reshape( rot90(ff),1,[]);
idx=idx+1;
end
%menu('','')

if (mod(i,100)==0)

clc;
i
end
end
end
end
end
label=ones(size(Data,1),1);
%%
% 
% load('fileellipseangle.mat')
% idx=1;
% for i=1:20000
% 
% for j=1:size(c{i}{2},1)
%     
%     data=c{i}{2}(j,:);
% r(idx)=max(data(3:4));
% idx=idx+1;
% end
% end
% histogram(r*2)