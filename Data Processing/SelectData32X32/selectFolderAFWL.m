function f=selectFolderAFWL(name)
try 
f=  imread(strcat('E:\Bases de datos\AFWL\aflw\data\flickr\0\',name));
catch
end
try
f=  imread(strcat('E:\Bases de datos\AFWL\aflw\data\flickr\2\',name));
catch
end
try
    f=  imread(strcat('E:\Bases de datos\AFWL\aflw\data\flickr\3\',name)); 
catch
end


end