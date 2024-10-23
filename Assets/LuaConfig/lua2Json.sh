#当前脚本的路径
echo "-------------转换开始-------------";
currentDir=$(cd $(dirname $0); pwd);
echo "${currentDir}";
#目标储存路径
rootPath=${currentDir%%/Assets*};
targetPath="${rootPath}/Assets/AssetBundleResource/config"
#lua配置文件的路径
cd config;
configDir=$(pwd);
echo "lua文件配置路径${configDir}";
echo "目标文件路径${targetPath}";

#开始遍历文件
for file in $(ls ${configDir}); 
do 
	if echo "$file" | grep -q -E '\.meta$'; then
		continue;
	fi
	#获取lua文件执行后的返回值
	fileName=${file%%.lua};
	lua $file "${targetPath}" "${fileName}.json"
done

echo "-------------转换结束-------------";
