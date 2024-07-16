import {View} from "react-native";

interface ISpacerProps {
    size: number
}

export default (props: ISpacerProps) => {
    return <View style={{height: props.size}}/>;
};