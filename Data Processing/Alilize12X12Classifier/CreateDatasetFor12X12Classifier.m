clear all; clc;
load('E:\Bases de datos\BasesDeDatosMatlab\FaceDetection12_16_32\tp12.mat');
Data=single(Data);
out1=[];
for i=1:6
load(strcat('E:\Bases de datos\AFWL\aflw\data\flickr\ImagenesSinRostros',num2str(i),'\falses12.mat'));
out1=[out1; output];
end
out1=out1(std(out1(:,1:100),0,2)~=0,:);
%Data= [Data(1:5:end,:); Data(2:5:end,:); Data(3:5:end,:); Data(4:5:end,:)];
out1=out1( 1: size(out1,1)/size(Data,1):end,:);
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


