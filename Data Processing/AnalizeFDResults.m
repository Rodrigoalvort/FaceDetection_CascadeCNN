clear all;clc;
load('StadisticsFD16X16X3.mat');
out=(Stadistics(:,2:3)');
target=Stadistics(:,1)';
target1hot=zeros(size(out));
target1hot(1,target==0)=1;
target1hot(2,target==1)=1;
%plotconfusion(target1hot,out);

for i=1:1000
thd=i*0.001;
outb=out(2,:)>thd;
success(i)=sum(target==outb)/length(out);
    
end
plot(0.001*(1:1000),success)
[~,idx]=max(success);