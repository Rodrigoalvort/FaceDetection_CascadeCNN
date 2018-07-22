function  ViewImagesConcatened(m, fp)

fp=rot90(reshape(input,m,m,3,[]),-1);
idx=1;
image=uint8([]);
for k=1:floor(size(fp,2)/2500)
for i=1:50
for j=1:50
image(m*(i-1)+1:m*i,m*(j-1)+1:m*j,:)=uint8(fp(:,:,:,idx));
idx=idx+1;
end
end
imshow((image));
menu('','')
end
end

