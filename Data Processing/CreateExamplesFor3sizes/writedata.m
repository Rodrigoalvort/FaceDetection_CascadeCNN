function writedata(f,m,fileID,label,rot)
f=imresize(f,[m m]);
e=rot90(f);
face=reshape(e,1,[]);
fprintf(fileID,'%d\t',label); 
for j=1:size(face,2) 
    fprintf(fileID,'%d\t',face(j)); 
end
fprintf(fileID,'\n'); 
if (label==1)
e=fliplr(f);

    if (rot==0)
e=rot90(e);
    elseif(rot==2)
e=rot90(e,-1);
 
    else
        
    end
face=reshape(e,1,[]);
fprintf(fileID,'%d\t',label); 
for j=1:size(face,2) 
    fprintf(fileID,'%d\t',face(j)); 
end
fprintf(fileID,'\n'); 
end


end
