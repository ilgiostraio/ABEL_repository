clear all
close all
Expressions(1)= xml_load('AU_Neutral.xml');
Expressions(2)= xml_load('AU_Anger.xml');
Expressions(3)= xml_load('AU_Disgust.xml');
Expressions(4)= xml_load('AU_Fear.xml');
Expressions(5)= xml_load('AU_Happiness.xml');
Expressions(6)= xml_load('AU_Sadness.xml');
Expressions(7)= xml_load('AU_Sleep.xml');
Expressions(8)= xml_load('AU_Surprise.xml');
% Expressions(9)= xml_load('Relax.xml');
% Expressions(10)= xml_load('Sleep.xml');

ECS=xml_load('ECS.xml');


numberofexpressions=(size(Expressions,2));
pleasures=zeros(1,numberofexpressions);
arousals=zeros(1,numberofexpressions);
dominances=zeros(1,numberofexpressions);

numbersofmotor=(size((Expressions(1,1).ServoMotorsList),2));
motorpositions=zeros(numbersofmotor,numberofexpressions);

value=0;

for i=1:numberofexpressions
pleasures(1,i)=str2double(Expressions(i).ECSCoord.Pleasure);
arousals(1,i)=str2double(Expressions(i).ECSCoord.Arousal);
dominances(1,i)=str2double(Expressions(i).ECSCoord.Dominance);


    for j=1:numbersofmotor
        value=str2double(Expressions(i).ServoMotorsList(1,j).ServoMotor.PulseWidth);
        if value>=0 % if the expression contains a valid value for this motor (otherwaise is -1)          
            motorpositions(j,i)=value;
        else %if this expressions does'nt use this motor the value is replaced with the neutral one (exp0)
            motorpositions(j,i)=str2double(Expressions(1).ServoMotorsList(1,j).ServoMotor.PulseWidth);
        end        
    end    
end

mesh(motorpositions)
continuousArousal= -1:.1:1;
continuousPleasure= -1:.1:1;
[p,a] = meshgrid(continuousPleasure,continuousArousal);

%pleasures=rand(8,1)*4-2;
%arousals=rand(8,1)*4-2;
%ecs=struct('Motor0',(zeros(size(continuousArousal,2),size(continuousPleasure,2))));
temp=zeros(size(continuousArousal,2),size(continuousPleasure,2));
%Interp = TriScatteredInterp(pleasures(:),arousals(:),aaa);

% for i= 1:numbersofmotor
%     motnumb=strcat('Motor',num2str(i-1));
%     ecs.(motnumb)=(zeros(size(continuousArousal,2),size(continuousPleasure,2)));     
% end

for i=1:numbersofmotor
    
    Interp = TriScatteredInterp(pleasures(:),arousals(:),motorpositions(i,:)','natural');
    temp = Interp(p,a);
    
    for k=1:size(temp,2); %interpolation of nan by rows
        if (size(find(~isnan(temp(k,:))),2))>=2 %if all nans
        temp(k,:)=naninterp(temp(k,:));
        end
    end
    
    for q=1:size(temp,1); %interpolation of nan by rows
        if (size(find(~isnan(temp(:,q))),1))>=2 %if all nans
        temp(:,q)=naninterp(temp(:,q));
        end
    end
    
    
    temp(isnan(temp))=-1;
     temp(temp<0)=-1;
     temp(temp>1)=1;
%     
    temp=round(temp.*1000)./1000;
    
    ECS.ECSMotorList(1,i).ECSMotor.ECSValues=temp;
    ECS.ECSMotorList(1,i).ECSMotor.SizeX=size(continuousPleasure,2);
    ECS.ECSMotorList(1,i).ECSMotor.SizeY=size(continuousArousal,2);
    
    
end

selectedmotor=7;
figure
surf(p,a,ECS.ECSMotorList(1,selectedmotor).ECSMotor.ECSValues),xlabel('Pleasure'),ylabel('Arousal');

hold on;
plot3(pleasures,arousals,motorpositions(selectedmotor,:),'o', 'Color',[0 0 0], 'LineWidth',2, 'MarkerFaceColor',[0 0 0]);
%title('Expressions');
grid on;
axis([-1 1 -1 1 0 1]);
circle([0,0],1,100,'--');

for i=1:numberofexpressions
    text(pleasures(i)+0.05,arousals(i)+0.05,motorpositions(selectedmotor,i)+0.05,(Expressions(i).Name), 'Color',[0 0 0], 'FontWeight','bold');
end

filename=sprintf('ECS_new.xml');
file=fopen(filename,'wt');
xmlstr=xml_format(ECS,'off','ECS');
fprintf(file,xmlstr);
fclose(file);



