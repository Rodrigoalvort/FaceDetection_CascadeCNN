%%AFW read faces :)
clear all; clc;
load('E:\Bases de datos\AFW\testimages\anno.mat');
for i=1:size(anno,1)
im=imread(strcat('E:\Bases de datos\AFW\testimages\',anno{i,1}));
data=anno{i,2};
for j=1:size(data,2)    
f=im(data{j}(3):data{j}(4),data{j}(1):data{j}(2),:);
imshow(f);  
end
end