%% selectFalses positives for new Classifier
clear all; clc;
load('E:\DeepLearning\cntk\Examples\Facedetection12X12X3-Weak60Angle\Output\OutputNodes.z.mat');
output=output(:,2)';
thd=0.215;
error=and(not(target'),output>=thd);







