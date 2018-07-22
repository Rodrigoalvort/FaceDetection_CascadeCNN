function  ViewImagesConcatened(m, fp, lim)

%fp=(reshape(fp,m,m,3,[]),-1);
fp=(reshape(fp,m,m,size(fp,1)/(m^2),[]));

idx=1;
image=uint8([]);
for k=1:floor(size(fp,4)/lim^2)
for i=1:lim
for j=1:lim
image(m*(i-1)+1:m*i,m*(j-1)+1:m*j,:)=uint8(fp(:,:,:,idx));
idx=idx+1;
end
end
imshow(image);
menu('','')
end
end

