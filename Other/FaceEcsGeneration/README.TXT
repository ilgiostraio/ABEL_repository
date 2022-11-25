
1) First copy the updated expressions in the folder Matlab\FaceEcsGeneration\ECS_expression with the name AU_[expression].xml
2) Open ecs_calculator.m
3) Add to path (Right click on the folder in "Current Folder") the folder containing the .xml expressions and all the other inside Matlab folder
4) Run ecs_calculator.m generating ecs.xml
5) Move ECS_new.xml in bin\debug and rename file as ECS.xml (before remember to rename the old file ecs.xml that is in the folder as ecs_old.xml)

Note:
- ECS.xml in the Matlab\FaceEcsGeneration\ folder is not the right xml to use but the template used by ecs_calculator.m


Last Update: 16-07-2014