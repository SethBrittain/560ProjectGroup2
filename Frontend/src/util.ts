interface User {
	id: number,
	organizationId: number,
	email: string,
	firstName: string,
	lastName: string,
	title: string,
	active: boolean,
	profilePhotoUrl: string,
	createdOn: Date,
	updatedOn: Date
}

interface DirectMessage {
	directMessageId: number,
	sender: User,
	recipient: User,
	message: string,
	createdOn: Date,
	updatedOn: Date
}

interface Channel {
	channelId: number,
	groupId: number,
	name: string,
	createdOn: Date,
	updatedOn: Date
}

interface ChannelMessage {
	channelMessageId: number,
	sender: User,
	channel: Channel,
	message: string,
	createdOn: Date,
	updatedOn: Date
}

export { User, DirectMessage, Channel, ChannelMessage };