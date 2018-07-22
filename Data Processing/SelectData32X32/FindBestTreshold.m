%% find best threshold
clear all; clc;
load('E:\DeepLearning\cntk\Examples\FaceDetection32X32X3\OutputNodes.z.mat');

output=double(output');
target=full(ind2vec(double(target')+1,2));
tn= output(2,target(1,:)==1);
tp= output(2,target(2,:)==1);
for i=1:1000
thd=i/1000;
tnr(i)=sum(tn<=thd)/length(tn);
tpr(i)=sum(tp>=thd)/length(tp);
errt(i)=sum(target(2,:)==(output(2,:)>=thd))/size(output,2);
end
close all;
subplot(1,2,1);
plot(tnr)
hold on
plot(tpr)
plot(errt)

grid;
legend('falses Positives','true Positives','error total')
ylim([0.9 1]);
[mm,idx]=max(errt);
plot(idx,mm,'o');
subplot(1,2,2);
plot(tnr,tpr);
grid;


%% 
clear all; clc;
load('E:\Bases de datos\BasesDeDatosMatlab\FaceDetection12_16_32\tptn32.mat');
input=output;
load('E:\DeepLearning\cntk\Examples\FaceDetection32X32X3\OutputNodes.z.mat');

falses= and(target==0,output(:,2)>0.8);
ViewImagesConcatened(32,input(falses,:)',10);

