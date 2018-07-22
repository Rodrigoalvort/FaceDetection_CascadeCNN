function BinaryReader()

fileID = fopen('magic4.dat','w');
e=rand(3,3);
fwrite(fileID,e,'double');
fclose(fileID);

fileID = fopen('magic4.dat');
A(1) = fread(fileID,1,'double')
A(2) = fread(fileID,1,'double')

fclose(fileID);
end