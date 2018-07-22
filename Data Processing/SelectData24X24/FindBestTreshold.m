%% find best threshold
clear all; clc;
%load('E:\DeepLearning\cntk\Examples\FaceDetection24X24X3\OutputNodes_z1.mat');
load('E:\DeepLearning\cntk\Examples\FaceDetection24X24X3V2\OutputNodes.z.mat');
load('OutputNodes.z.mat');

%load('E:\DeepLearning\OutputNodes.z.mat');

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

im=imread('C:\Users\Ideapad 300\Google Drive\database\s4\CI_49.png');
imshow(im);
hold on;
X=[957 1011]; Y=[527 497];
plot(X,Y,'LineWidth',2);
plot(X,[Y(1) Y(1)],'LineWidth',2);
r=atan2d(527-497,1011-957);
R=[cosd(r) sind(r);-sind(r) cosd(r)];

Size=floor(abs(R)*[size(im,1) size(im,2)]'+0.5);
p1=  R^1*[1 0 -size(im,1)/2;0 1 -size(im,2)/2]*[Y(1); X(1) ;1]+ Size/2;                             
im2=imrotate(im,-r);
figure;
imshow(im2);
%hold on;
%plot(p1(2),p1(1),'o');
