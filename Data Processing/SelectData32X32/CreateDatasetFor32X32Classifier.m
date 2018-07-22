clear all; clc;
load('E:\Bases de datos\BasesDeDatosMatlab\FaceDetection12_16_32\tp32.mat');
Data=single(Data);
out1=[];
for i=1:6
load(strcat('E:\Bases de datos\AFWL\aflw\data\flickr\ImagenesSinRostros',num2str(i),'\falses12-24-32.mat'));
out1=[out1; output];
end
out([964 965 1199 1204 1205 1721 1724 1725 1733 1748 1788 1789 2002 2096 2336:2337 4116],:)=255;
out1=out1(std(out,0,2)~=0,:);

for i=1:5
load(strcat('E:\Bases de datos\ImageNet\Imagenes',num2str(i),'\falses12-24-32.mat'));
out1=[out1; output];
end


%Data= [Data(1:5:end,:); Data(2:5:end,:); Data(3:5:end,:); Data(4:5:end,:)];
target=single( [zeros(size(out1,1),1);ones(size(Data,1),1)]);
output=single([out1;Data]); clear Data; clear out1;
[~,idx]=sort(rand(length(target),1));
output=output(idx,:);
target=target(idx);
% OutputEval=output(290000:end,:);
% targetEval=target(290000:end,:);
% 
% output=output(1:290000,:);
% target=target(1:290000,:);

%ViewImagesConcatened(24,output(target==1,:)',25);


