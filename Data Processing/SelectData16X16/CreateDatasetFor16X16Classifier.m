clear all; clc;
load('E:\Bases de datos\BasesDeDatosMatlab\FaceDetection12_16_32\falses12.mat');
%ViewImagesConcatened(16,output');

load('E:\Bases de datos\BasesDeDatosMatlab\FaceDetection12_16_32\falses12_2.mat');
%ViewImagesConcatened(16,fp);

output=[output;fp']; clear fp;
r=single(20*(rand(size(output))-0.5));
output=single(uint8(output+r));
load('E:\Bases de datos\BasesDeDatosMatlab\FaceDetection12_16_32\tp16V2.mat');
%ViewImagesConcatened(16,ans);
 Data=[ Data; reshape(flipud(reshape(Data',16,16,3,[])),768,[])']; 
%ViewImagesConcatened(16,Data',50);
target=single( [zeros(size(output,1),1);ones(size(Data,1),1)]);
output=single([output;Data]);

[~,idx]=sort(rand(length(target),1));
output=output(idx,:);
target=target(idx);
%ViewImagesConcatened(16,output(target==1,:)');


output=(output(:,1:256) +output(:,257:512)+output(:,513:768))/3;
