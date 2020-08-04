-- constants / var that will change but it's mostly config stuff
if not isfile("nearby_notif.cfg") then
    writefile("nearby_notif.cfg","3")
end
NEAR_DISTANCE = tonumber(readfile("nearby_notif.cfg")) -- Distance that is considered to be near a player
-- constants vars
local LocalPlayer = game.Players.LocalPlayer
local OnNewMessage = game.ReplicatedStorage.DefaultChatSystemChatEvents.OnMessageDoneFiltering
local Remotes = game.ReplicatedStorage.Remotes
local HttpService  = game:GetService("HttpService")


-- functions
function get_player_rap(user_id)
    return Remotes.UserLimiteds.GetUserRAP:InvokeServer(user_id).rap
end

function get_user_value( user_id )
    return Remotes.UserLimiteds.GetUserRAP:InvokeServer(user_id).value
end
--// websocket connection
local saveSettingSocket = syn.websocket.connect("ws://localhost:24892/custom?channel=settingSaveRequest")
local PlayerDataSocket = syn.websocket.connect("ws://localhost:24892/custom?channel=PlayerDataSocket")

saveSettingSocket.OnMessage:connect(function(new_near_distance)
    if not tonumber(new_near_distance) then
        saveSettingSocket:Send("LETTER_ERROR")
    else
        NEAR_DISTANCE = tonumber(new_near_distance);
        writefile("nearby_notif.cfg", new_near_distance)
    end
end)

OnNewMessage.OnClientEvent:connect(function(MessageTable,MessageChannel)
    if MessageTable.FromSpeaker == LocalPlayer.Name then return end
    local SpeakerInstance = game.Players[MessageTable.FromSpeaker]
    local SpeakerUserId = MessageTable.SpeakerUserId;
    local SpeakerName = MessageTable.FromSpeaker
    local SpeakerRAP = get_player_rap(SpeakerUserId)
    local SpeakerValue = get_user_value(SpeakerUserId)
    local SpeakerMessage = MessageTable.Message
    if LocalPlayer:DistanceFromCharacter(SpeakerInstance.Character.PrimaryPart.Position) <= NEAR_DISTANCE then
        local SpeakerData = HttpService:JSONEncode{
            Username = SpeakerName,
            UserId = SpeakerUserId,
            RecentAveragePrice = SpeakerRAP,
            Value = SpeakerValue;
            Message = SpeakerMessage;
            AvatarThumb = "https://www.roblox.com/Thumbs/Avatar.ashx?x=420&y=420&userid=" .. SpeakerUserId 
        } 
        PlayerDataSocket:Send(SpeakerData)
    end
end)