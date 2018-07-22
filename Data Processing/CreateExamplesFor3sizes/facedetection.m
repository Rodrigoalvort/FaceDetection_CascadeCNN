%facedetector  Lasthope
clear all; clc;
%load('fileandellipse.mat');
load('fileellipseangle.mat')
s=-1;
ang=60*pi/180;
idx=1;
for i=1:20000
data=c{i}{2};
if (size(c{i}{2},1)==1)
if (abs(c{i}{3}(3))<ang)
if (2*max(data(3:4))>32)
f=selectFolderAFWL(c{i}{1});
% if (size(f,3)==3)  
%     f=rgb2gray(f);
% end

if (size(f,3)>1)
m=16;
x=floor(data(1));y=floor(data(2)); r=floor(max(data(3:4)));                                  %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% 
T=max(y-r,1); B=min(y+r,size(f,1)); L=max(x-r,1); R=min(x+r,size(f,2));

ff=f(T:B,L:R,:);%imshow(ff)

Example(idx)=CreateExample(ff,1);idx=idx+1; % subplot(2,3,1);  %imshow(ff);%face
Example(idx)=CreateExample(ExpoTrans( ff,0.3),1);idx=idx+1; %subplot(2,3,2); imshow(ExpoTrans( ff,0.3)); %face 
Example(idx)=CreateExample(ExpoTrans( ff,0.4),1);idx=idx+1; %subplot(2,3,3); imshow(ExpoTrans( ff,0.4));%face
Example(idx)=CreateExample(ExpoTrans( ff,2.5),1);idx=idx+1; %subplot(2,3,4); imshow(ExpoTrans( ff,2.5));%face
Example(idx)=CreateExample(ExpoTrans( ff,1.5),1);idx=idx+1; %subplot(2,3,5); imshow(ExpoTrans( ff,3));%face
Example(idx)=CreateExample(ExpoTrans( ff,3.0),1);idx=idx+1;   %subplot(2,3,6);   imshow(ExpoTrans( ff,1.5));%face


Example(idx)=CreateExample(f(B:end,L:R,:)   ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(B:end,1:L,:)   ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(B:end,R:end,:) ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(1:T,L:R,:)     ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(:,1:L,:)       ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(:,R:end,:)     ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(B:end,:,:)     ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(1:T,:,:)       ,0);idx=idx+1;%noface

Example(idx)=CreateExample(f(B:2:end,L:2:R,:)   ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(B:2:end,1:2:L,:)   ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(B:2:end,R:2:end,:) ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(1:2:T,L:2:R,:)     ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(1:2:end,1:2:L,:)   ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(1:2:end,R:2:end,:) ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(B:2:end,1:2:end,:) ,0);idx=idx+1;%noface
Example(idx)=CreateExample(f(1:2:T,1:2:end,:)   ,0);idx=idx+1;%noface


if (mod(i,500)==0)
i
end
end
end
end
end
end



%%
%%show examples antees de inciar leer el txt que esta presente  en
%%ucireaderdataset
% label=unnamed(:,1);
% data=unnamed((logical(label)),2:3073);
% data=reshape(data',32,32,3,[]);
% size1=size(data,4)/8;
% imagen=zeros(32*8,32*size1,3);
% idx=1;
% for j=1:size1
% for i=1:8
% imagen(32*(i-1)+1:32*i,32*(j-1)+1:32*j,:)=(data(:,:,:,idx));
% % if (i>4 & i<7)    
% % imagen(32*(i-1)+1:32*i,32*(j-1)+1:32*j,:)=rot90(data(:,:,:,idx),-2);
% % 
% % end
% % if (i>2 & i<5)    
% % imagen(32*(i-1)+1:32*i,32*(j-1)+1:32*j,:)=rot90(data(:,:,:,idx),1);
% % 
% % end
% idx=idx+1;
% end
% end
% imshow(uint8(imagen),[])
% clc;
% imagen=imresize(imagen,[size(imagen,1) size(imagen,2)]*2);
% imshow(uint8(imagen),[])

    