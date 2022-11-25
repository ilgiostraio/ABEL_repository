x = rand(8,1)*4-2;
y = rand(8,1)*4-2;
z = x.*exp(-x.^2-y.^2);

F = TriScatteredInterp(x,y,z);

ti = -1:.1:1;
[qx,qy] = meshgrid(ti,ti);
qz = F(qx,qy);



mesh(qx,qy,qz);
%hold on;
%plot3(x,y,z,'o');

filename=sprintf('dat.txt');
    file=fopen(filename,'wt');
    for i = 0:length(ti)
        for j= 0:length(ti)
        fprintf(file,'%f\t',qz);
        end
        fprintf(file,'\r\n');
    end
        
	fclose(file);
    
    
    
    
    filename=sprintf('ECS.xml');
    file=fopen(filename,'wt');
    xmlstr=xml_format(ECS,'off','ECS');
    fprintf(file,xmlstr);
    fclose(file);



    
