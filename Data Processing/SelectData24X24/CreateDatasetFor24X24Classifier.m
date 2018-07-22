clear all; clc;
load('E:\Bases de datos\BasesDeDatosMatlab\FaceDetection12_16_32\tp24.mat');


load('E:\Bases de datos\BasesDeDatosMatlab\FaceDetection12_16_32\falses12-24.mat');
target=single( [zeros(size(output,1),1);ones(size(Data,1),1)]);
output=single([output;Data]); clear Data;
[~,idx]=sort(rand(length(target),1));
output=output(idx,:);
target=target(idx);
% OutputEval=output(290000:end,:);
% targetEval=target(290000:end,:);
% 
% output=output(1:290000,:);
% target=target(1:290000,:);

%ViewImagesConcatened(24,output(target==1,:)',25);


