local config =  {
    config = {
        ["101"] = {name = "模型1", url = "", colorGroupList = {1,3,6}, specialList = {}, containerList = {1}, colorCount = 2},
        ["102"] = {name = "模型2", url = "", colorGroupList = {1,3,6}, specialList = {1003,1004}, containerList = {1,2}, colorCount = 3},
        ["103"] = {name = "模型3", url = "", colorGroupList = {1,3,6}, specialList = {1003,1004}, containerList = {1,2,3}, colorCount = 4},
    },
}
 
targetPath = arg[1];
fileName = arg[2];

--数据转json
function convert(val)
    fileDir = io.open(targetPath, "rb");
    if fileDir then 
        fileDir:close();
    else
        os.execute("mkdir -p ".. targetPath);
    end
    local cjson = require "cjson"
    local jsonData = cjson.encode(val)
    --保存
    local file = io.open(targetPath.."/"..fileName, "w");
    file:write(jsonData);
    file:close();

    return jsonData;
end

convert(config);