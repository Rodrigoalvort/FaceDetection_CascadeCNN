clear all; clc;
%% obtener archivos de 12X12 
idx=1;
Data=single(zeros(633688,432));
for j=1:3
load(strcat('E:\Bases de datos\BasesDeDatosMatlab\FaceDetection12_16_32\FD_p',num2str(j),'.mat'));
for i=1:length(FD1)
Data(idx,:)=single(FD1(i).image12); 
label(idx)=FD1(i).label;idx=idx+1;
Data(idx,:)=single(FD1(i).image12f); 
label(idx)=FD1(i).label;idx=idx+1;
if (mod(i,1000)==0)
i
end

end
clear FD1;
end
Data=Data';
label=single(label)';

%% obtener archivos de 16X16 

clear all; clc;
idx=1;
Data=single(zeros(633688,768));
for j=1:3
load(strcat('E:\Bases de datos\BasesDeDatosMatlab\FaceDetection12_16_32\FD_p',num2str(j),'.mat'));
for i=1:length(FD1)
Data(idx,:)=single(FD1(i).image16); 
label(idx)=FD1(i).label;idx=idx+1;
Data(idx,:)=single(FD1(i).image16f); 
label(idx)=FD1(i).label;idx=idx+1;
if (mod(i,1000)==0)
i
end

end
clear FD1;
end
Data=Data';
label=single(label)';


%% obtener archivos de 32X32 

clear all; clc;
idx=1;
%Data=single(zeros(633688,3072));
Data = uint8(0);
Data(633688,3072) = Data;
for j=1:3
load(strcat('E:\Bases de datos\BasesDeDatosMatlab\FaceDetection12_16_32\FD_p',num2str(j),'.mat'));
for i=1:length(FD1)
Data(idx,:)=single(FD1(i).image32); 
label(idx)=FD1(i).label;idx=idx+1;
Data(idx,:)=single(FD1(i).image32f); 
label(idx)=FD1(i).label;idx=idx+1;
if (mod(i,1000)==0)
i
end

end
clear FD1;
end
Data=Data';
label=single(label)';
