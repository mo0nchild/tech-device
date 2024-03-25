/* eslint-disable prettier/prettier */
import React from 'react';
import * as JqxKnob from 'jqwidgets-scripts/jqwidgets-react-tsx/jqxknob';

type KnobValue = number | (number | undefined)[] | undefined;

export type GeneratorProp = {
    onValueChange(value: number): void;
    labelFormater(value: string): string;
    readonly componentName: string;
    readonly maximum: number;
    readonly minimum: number;
    readonly step: number;
};
class GeneratorKnob extends React.Component<GeneratorProp, {value: number}> {
    
    private readonly knobProp: JqxKnob.IKnobProps;

    public constructor(props: GeneratorProp) {
        super(props);
        this.knobProp = {
            marks: {
                colorProgress: '#444', colorRemaining: '#FFF',
                majorInterval: this.props.step / 4, majorSize: '1%', minorInterval: 5,
                offset: '75%', size: '1%', thickness: 2
            },
            labels: {
                formatFunction: (label: string | number): string | number => {
                    return label;
                },
                offset: '90%', step: this.props.step, visible: true,
                style: { fill: '#fff' }
            },
            progressBar: {
                offset: '0%', size: '70%',
            },
            pointer: {
                offset: '50%', size: '60%',
                style: { fill: '#ff6126', stroke: '#444' },
                thickness: 25, type: 'arrow'
            },
            spinner: {
                innerRadius: '65%',
                marks: {
                    colorProgress: '#fff',
                    colorRemaining: '#fff',
                    majorInterval: 20,
                    majorSize: '5%',
                    minorInterval: 20,
                    offset: '68%',
                    size: '1%',
                    thickness: 4,
                    type: 'circle',
                },
                outerRadius: '70%',
                style: { fill: '#444', stroke: '#000' }
            },
            dial: {
                innerRadius: '0%',
                outerRadius: '46%',
                style: { fill: '#ff6126', stroke: '#000' }
            }
        };
        this.state = { value: 0 };
    }
    private onValueChanged = (_, newValue: KnobValue): boolean => {
        this.setState({
            value: newValue as number
        });
        this.props.onValueChange(this.state.value);
        return true;
    };
    public override render(): React.ReactElement {
        const { labelFormater, componentName, minimum, maximum } = this.props;
        return (
            <div style={{
                color: '#444', display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
                fontSize: 16, fontWeight: 'bold'
            }}>
                <p style={{ fontSize: 20, fontWeight: 'bold'}}>{componentName}</p>
                <JqxKnob.default height={220} width={220}
                    min={minimum} max={maximum} 
                    startAngle={120} endAngle={480} step={0.1}
                    dragStartAngle={120} dragEndAngle={420}
                    snapToStep={true} rotation={'clockwise'}
                    marks={this.knobProp.marks} 
                    labels={this.knobProp.labels}
                    progressBar={this.knobProp.progressBar} 
                    pointer={this.knobProp.pointer}
                    spinner={this.knobProp.spinner} 
                    dial={this.knobProp.dial} 
                    changing={this.onValueChanged}
                />
                <p>Значение: {labelFormater(this.state.value.toFixed(2))}</p>
            </div>
        );
    }
}
export default GeneratorKnob;