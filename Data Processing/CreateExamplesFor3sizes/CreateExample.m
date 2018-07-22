function [imagesvector] = CreateExample(image,label)
%% para 12
imagesvector.label=label;
image12=imresize(image,[12 12]);
e=rot90(image12);
imagesvector.image12=reshape(e,1,[]);
e=rot90(fliplr(image12));
imagesvector.image12f=reshape(e,1,[]);

%% para 16
image16=imresize(image,[16 16]);
e=rot90(image16);
imagesvector.image16=reshape(e,1,[]);
e=rot90(fliplr(image16));
imagesvector.image16f=reshape(e,1,[]);

%% para 32
image32=imresize(image,[32 32]);
e=rot90(image32);
imagesvector.image32=reshape(e,1,[]);
e=rot90(fliplr(image32));
imagesvector.image32f=reshape(e,1,[]);

end





