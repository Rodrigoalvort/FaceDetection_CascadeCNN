%% complemento de facedetection 
m=24;
label=unnamed(:,1);
data=unnamed((logical(label)),2:m*m*3+1);
%data=unnamed(not(logical(label)),2:m*m*3+1);

data=reshape(data',m,m,3,[]);
size1=size(data,4)/8;
imagen=zeros(m*8,m*size1,3);
idx=1;
for j=1:size1
for i=1:8
imagen(m*(i-1)+1:m*i,m*(j-1)+1:m*j,:)=rot90(data(:,:,:,idx),-1);
% if (i>4 & i<7)    
% imagen(32*(i-1)+1:32*i,32*(j-1)+1:32*j,:)=rot90(data(:,:,:,idx),-2);
% 
% end
% if (i>2 & i<5)    
% imagen(32*(i-1)+1:32*i,32*(j-1)+1:32*j,:)=rot90(data(:,:,:,idx),1);
% 
% end
idx=idx+1;
end
end
imshow(uint8(imagen),[])
clc;
imagen=imresize(imagen,[size(imagen,1) size(imagen,2)]*2);
imshow(uint8(imagen),[])
